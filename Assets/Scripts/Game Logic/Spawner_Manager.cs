using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;


/// <summary>
/// Manages pet spawning in the ecosystem by polling Firebase spawn requests.
/// Enforces maximum pet population limit and handles pet instantiation.
/// </summary>
public class Spawner_Manager : MonoBehaviour
{
    public static Spawner_Manager Instance { get; private set; }

    [Header("Settings")]
    /// <summary>
    /// Indicates whether the ecosystem actively processes spawn requests or not.
    /// </summary>
    [SerializeField] bool ecosystemActive = true;
    /// <summary>
    /// Maximum number of pets allowed in the ecosystem simultaneously.
    /// </summary>
    [SerializeField] int maxPets = 20;

    [Header("Spawn References")]
    /// <summary>
    /// Available spawn points in the world for pets. 
    /// </summary>
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] GameObject[] petPrefabsArray;
    Dictionary<string, GameObject> petPrefabs;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        // Build prefab lookup dictionary for efficient access by name
        petPrefabs = new Dictionary<string, GameObject>();
        foreach (var prefab in petPrefabsArray)
            petPrefabs[prefab.name] = prefab;
    }

    void Start()
    {
        StartCoroutine(InitFirebaseREST());
    }

    /// <summary>
    /// Initializes Firebase connection and starts polling for spawn requests.
    /// Clears any leftover ecosystem data from previous sessions.
    /// </summary>
    IEnumerator InitFirebaseREST()
    {
        // Wait for FirebaseREST instance
        while (FirebaseREST.Instance == null)
            yield return null;

        Debug.Log("Spawner connected to Firebase REST");

        // Clear leftover ecosystem data
        FirebaseREST.Instance.DeleteData("ecosystem");

        // Start polling spawn requests
        StartCoroutine(PollSpawnRequests());
    }

    /// <summary>
    /// Continuously polls Firebase for spawn requests every 2 seconds.
    /// Processes requests if ecosystem is active and capacity allows.
    /// </summary>
    IEnumerator PollSpawnRequests()
    {
        while (true)
        {
            if (!ecosystemActive)
            {
                yield return new WaitForSeconds(2f);
                continue;
            }

            FirebaseREST.Instance.GetData("ecosystem/spawnRequests", json =>
            {
                if (string.IsNullOrEmpty(json) || json == "null") return;

                try
                {
                    var dict = Json.Deserialize(json) as Dictionary<string, object>;
                    if (dict == null) return;

                    foreach (var kvp in dict)
                    {
                        string key = kvp.Key;
                        var value = kvp.Value as Dictionary<string, object>;
                        if (value == null) continue;

                        string petName = value.ContainsKey("name") ? value["name"] as string : null;
                        string petType = value.ContainsKey("type") ? value["type"] as string : null;

                        if (string.IsNullOrEmpty(petName) || string.IsNullOrEmpty(petType)) continue;

                        // Enforce population limit
                        if (Pet_Manager.Instance.Pets.Count >= maxPets)
                        {
                            Debug.Log($"Ecosystem full ({maxPets}), ignoring spawn request {petName}");
                            FirebaseREST.Instance.DeleteData($"ecosystem/spawnRequests/{key}");
                            continue;
                        }

                        SpawnPet(petName, petType);
                        FirebaseREST.Instance.DeleteData($"ecosystem/spawnRequests/{key}");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Failed to parse spawn requests: " + e);
                }
            });
            // poll every 2 seconds
            yield return new WaitForSeconds(2f); 
        }
    }

    /// <summary>
    /// Spawns a pet at a random spawn point and registers it in Firebase.
    /// Generates unique ID for tracking and assigns pet name.
    /// </summary>
    /// <param name="petName">Display name for the pet.</param>
    /// <param name="petType">Pet prefab type matching prefab name.</param>
    public void SpawnPet(string petName, string petType)
    {
        if (petPrefabs == null || !petPrefabs.ContainsKey(petType) || spawnPoints == null)
        {
            Debug.LogError($"Missing prefab or spawn point for {petType}");
            return;
        }

        string petID = System.Guid.NewGuid().ToString();

        // Add to Firebase
        string json = $"{{\"name\":\"{petName}\",\"type\":\"{petType}\",\"status\":\"active\"}}";
        FirebaseREST.Instance.SetData($"ecosystem/pets/{petID}", json);

        // Spawn pet at a random point
        Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject instance = Instantiate(petPrefabs[petType], randomPoint.position, Quaternion.identity);
        var animal = instance.GetComponent<BaseAnimal>();
        animal.SetPetName(petName);
        animal.SetPetID(petID);

        Debug.Log($"Spawned {petType} ({petName}) with ID: {petID}");
    }
}
