using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Collections;

public class Spawner_Manager : MonoBehaviour
{
    public static Spawner_Manager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] bool ecosystemActive = true;
    [SerializeField] int maxPets = 20;

    [Header("Spawn References")]
    [SerializeField] Transform SpawnPoint;
    [SerializeField] GameObject[] petPrefabsArray;

    private Dictionary<string, GameObject> petPrefabs;
    private DatabaseReference db;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        petPrefabs = new Dictionary<string, GameObject>();
        foreach (var prefab in petPrefabsArray)
        {
            petPrefabs[prefab.name] = prefab;
        }
    }

    void Start()
    {
        StartCoroutine(InitFirebase());
    }

    IEnumerator InitFirebase()
    {
        while (!FirebaseInit.Ready) yield return null;

        db = FirebaseInit.db;
        Debug.Log("Spawner connected to Firebase");

        // Clear any leftover pets or queue on ecosystem start
        db.Child("ecosystem").RemoveValueAsync().ContinueWithOnMainThread(t =>
        {
            if (t.IsCompleted) Debug.Log("Cleared old Firebase ecosystem data");
        });

        ListenForSpawnRequests();
    }

    void ListenForSpawnRequests()
    {
        if (db == null) return;
        db.Child("ecosystem").Child("spawnRequests").ChildAdded += HandleSpawnRequest;
    }

    void HandleSpawnRequest(object sender, ChildChangedEventArgs args)
    {
        if (args.Snapshot == null || !args.Snapshot.Exists) return;
        if (!ecosystemActive)
        {
            args.Snapshot.Reference.RemoveValueAsync();
            return;
        }

        string petName = args.Snapshot.Child("name").Value?.ToString();
        string petType = args.Snapshot.Child("type").Value?.ToString();

        if (string.IsNullOrEmpty(petName) || string.IsNullOrEmpty(petType)) return;

        // Check pet limit
        if (Pet_Manager.Instance.Pets.Count >= maxPets)
        {
            Debug.Log($"Ecosystem full ({maxPets}), ignoring spawn request {petName}");
            args.Snapshot.Reference.RemoveValueAsync();
            return;
        }

        // Spawn
        SpawnPet(petName, petType);
        args.Snapshot.Reference.RemoveValueAsync();
    }

    public void SpawnPet(string petName, string petType)
    {
        if (petPrefabs == null || !petPrefabs.ContainsKey(petType) || SpawnPoint == null)
        {
            Debug.LogError($"Missing prefab or spawn point for {petType}");
            return;
        }

        // Unique ID for Firebase tracking
        string petID = System.Guid.NewGuid().ToString();

        // Add to Firebase
        db.Child("ecosystem").Child("pets").Child(petID).SetRawJsonValueAsync(
            $"{{\"name\":\"{petName}\",\"type\":\"{petType}\",\"status\":\"active\"}}"
        );

        // Spawn prefab
        GameObject instance = Instantiate(petPrefabs[petType], SpawnPoint.position, Quaternion.identity);
        var animal = instance.GetComponent<BaseAnimal>();
        animal.SetPetName(petName);
        animal.SetPetID(petID);

        Debug.Log($"Spawned {petType} ({petName}) with ID: {petID}");
    }
}
