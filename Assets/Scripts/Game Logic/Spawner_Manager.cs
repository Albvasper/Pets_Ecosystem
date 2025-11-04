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
    int pendingSpawns = 0;

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
    /// Spawns a pet at a random spawn point and registers it in Firebase.
    /// Generates unique ID for tracking and assigns pet name.
    /// </summary>
    /// <param name="petName">Display name for the pet.</param>
    /// <param name="petType">Pet prefab type matching prefab name.</param>
    public void SpawnPet(string petName, string petType)
    {
        // Check total count including pending spawns
        if (Pet_Manager.Instance.Pets.Count + pendingSpawns >= maxPets)
        {
            Debug.Log($"Ecosystem full ({maxPets}), cannot spawn {petName}");
            return;
        }

        if (petPrefabs == null || !petPrefabs.ContainsKey(petType) || spawnPoints == null)
        {
            Debug.LogError($"Missing prefab or spawn point for {petType}");
            return;
        }

        pendingSpawns++;

        string petID = System.Guid.NewGuid().ToString();

        // Add to Firebase
        string json = $"{{\"name\":\"{petName}\",\"type\":\"{petType}\",\"status\":\"active\",\"isZombie\":false}}";    
        FirebaseREST.Instance.SetData($"ecosystem/pets/{petID}", json);

        // Spawn pet 
        SpawnPetAtRandomPoint(petName, petType, petID);
        
        pendingSpawns--;

        Debug.Log($"Spawned {petType} ({petName}) with ID: {petID}");
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

        // Check database for old pets and spawn them
        CheckForOldPets();
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
    /// Checks Firebase for existing pets from previous sessions and spawns them.
    /// Called on ecosystem initialization to restore pet population.
    /// </summary>
    void CheckForOldPets()
    {
        FirebaseREST.Instance.GetData("ecosystem/pets", json =>
        {
            if (string.IsNullOrEmpty(json) || json == "null") return;

            var dict = Json.Deserialize(json) as Dictionary<string, object>;
            if (dict == null) return;

            int restored = 0;
            foreach (var kvp in dict)
            {
                if (Pet_Manager.Instance.Pets.Count + pendingSpawns >= maxPets) break;

                var petData = kvp.Value as Dictionary<string, object>;
                if (petData == null) continue;

                string petName = petData.ContainsKey("name") ? petData["name"] as string : "Unknown";
                string petType = petData.ContainsKey("type") ? petData["type"] as string : null;
                bool isZombie = petData.ContainsKey("isZombie") && (bool)petData["isZombie"];

                pendingSpawns++;
                if (SpawnPetAtRandomPoint(petName, petType, kvp.Key, isZombie) != null)
                    restored++;
                pendingSpawns--;
            }

            Debug.Log($"Restored {restored} pets from Firebase");
        });
    }

    GameObject SpawnPetAtRandomPoint(string petName, string petType, string petID, bool isZombie = false)
    {
        if (!petPrefabs.ContainsKey(petType) || spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogError($"Cannot spawn pet: Missing prefab or spawn point for {petType}");
            return null;
        }

        Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        
        // Add random offset to prevent pets spawning on top of each other
        Vector2 offset = Random.insideUnitCircle * 2f;
        Vector3 spawnPosition = randomPoint.position + new Vector3(offset.x, offset.y, 0);
        
        GameObject instance = Instantiate(petPrefabs[petType], spawnPosition, Quaternion.identity);
        var animal = instance.GetComponent<BaseAnimal>();
        animal.SetPetName(petName);
        animal.SetPetID(petID);
        
        if (isZombie)
        {
            animal.IsZombie = true;
            Pet_Manager.Instance.AddToZombiePopulation();
            animal.Animator.TurnIntoZombie();
        }
        
        return instance;
    }
}