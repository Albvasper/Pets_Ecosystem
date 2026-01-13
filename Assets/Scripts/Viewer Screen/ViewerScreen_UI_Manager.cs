using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Manages viewer UI flow for connecting to ecosystem, queueing, and spawning pets.
/// Handles capacity checking, queue position tracking, and pet assignment.
/// </summary>
public class ViewerScreenUIManager : MonoBehaviour
{
    const int MaxPets = 20;
    [Header("UI Screens")]
    [SerializeField] GameObject welcomeScreen;
    [SerializeField] GameObject queueScreen;
    [SerializeField] GameObject spawnScreen;
    [SerializeField] GameObject successScreen;

    [Header("UI Elements")]
    public TMP_InputField publicKeyInput;
    public TMP_Text publicKeyFeedbackText;
    public TMP_InputField nameInput;
    public GameObject feedbackBox;
    public TMP_Text feedbackText;
    public TMP_Text queuePositionText;
    public Animator petPreviewAnimator;

    [Header("Pet Animator Controllers")]
    public RuntimeAnimatorController catAnimator;
    public RuntimeAnimatorController dogAnimator;
    public RuntimeAnimatorController wolfAnimator;
    public RuntimeAnimatorController deerAnimator;
    public RuntimeAnimatorController bearAnimator;
    public RuntimeAnimatorController tigerAnimator;
    public RuntimeAnimatorController pikachuAnimator;
    public RuntimeAnimatorController bulbasaurAnimator;
    public RuntimeAnimatorController charizardAnimator;
    public RuntimeAnimatorController squirtleAnimator;
    public RuntimeAnimatorController kabutoAnimator;

    // Unique ID for each viewer.
    string viewerID;
    // Randomly assigned pet.
    string assignedPet;
    bool isInQueue = false;

    void Awake()
    {
        welcomeScreen.SetActive(true);
        queueScreen.SetActive(false);
        spawnScreen.SetActive(false);
        successScreen.SetActive(false);
    }

    void Start()
    {
        viewerID = System.Guid.NewGuid().ToString();
        StartCoroutine(PollEcosystemForQueue());
    }

    /// <summary>
    /// Validates public key and initiates connection to ecosystem.
    /// Checks capacity and either shows spawn screen or adds viewer to queue.
    /// </summary>
    public void ConnectToEcosystem()
    {
        if (string.IsNullOrEmpty(publicKeyInput.text))
        {
            publicKeyFeedbackText.text = "Introduce a key!";
            return;
        }

        welcomeScreen.SetActive(false);
        queueScreen.SetActive(false);
        spawnScreen.SetActive(false);
        successScreen.SetActive(false);

        CheckEcosystemCapacity();
    }
    
    /// <summary>
    /// Validates pet name and submits spawn request to Firebase.
    /// Creates a spawn request entry for the spawner manager to process.
    /// </summary>
    public void NameAndSpawnPet()
    {
        string petName = nameInput.text.Trim();
        if (string.IsNullOrEmpty(petName))
        {
            feedbackBox.SetActive(true);
            feedbackText.text = "Enter a name!";
            return;
        }

        CheckCapacityBeforeSpawn(petName);
    }

    /// <summary>
    /// Displays success screen after pet spawn request is submitted.
    /// </summary>
    public void GoToSuccessScreen()
    {
        welcomeScreen.SetActive(false);
        queueScreen.SetActive(false);
        spawnScreen.SetActive(false);
        successScreen.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
    /// <summary>
    /// Removes viewer from queue and returns to welcome screen.
    /// </summary>
    public void CancelQueue()
    {
        welcomeScreen.SetActive(true);
        queueScreen.SetActive(false);
        spawnScreen.SetActive(false);
        successScreen.SetActive(false);

        isInQueue = false;
        FirebaseREST.Instance.DeleteData($"ecosystem/queue/{viewerID}");
    }
    
    /// <summary>
    /// Continuously monitors ecosystem capacity while viewer is in queue.
    /// Automatically moves viewer to spawn screen when space becomes available.
    /// </summary>
    IEnumerator PollEcosystemForQueue()
    {
        while (true)
        {
            if (isInQueue)
            {
                FirebaseREST.Instance.GetData($"ecosystem/pets", json =>
                {
                    int petCount = 0;
                    if (!string.IsNullOrEmpty(json) && json != "null")
                    {
                        var dict = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;
                        if (dict != null) petCount = dict.Count;
                    }
                    // Check if ecosystem has space
                    if (petCount < MaxPets)
                    {
                        // Move from queue to spawn screen
                        spawnScreen.SetActive(true);
                        AssignRandomPet();
                        feedbackBox.SetActive(false);
                        feedbackText.text = "";
                        nameInput.text = "";
                        queueScreen.SetActive(false);
                        isInQueue = false;

                        FirebaseREST.Instance.DeleteData($"ecosystem/queue/{viewerID}");
                    }
                });
            }

            yield return new WaitForSeconds(2f);
        }
    }

    /// <summary>
    /// Checks current ecosystem pet count and routes viewer accordingly.
    /// Shows spawn screen if under capacity, otherwise adds to queue.
    /// </summary>
    void CheckEcosystemCapacity()
    {
        FirebaseREST.Instance.GetData("ecosystem/pets", petsJson =>
        {
            FirebaseREST.Instance.GetData("ecosystem/spawnRequests", requestsJson =>
            {
                int petCount = 0;
                int requestCount = 0;

                if (!string.IsNullOrEmpty(petsJson) && petsJson != "null")
                {
                    var dict = MiniJSON.Json.Deserialize(petsJson) as Dictionary<string, object>;
                    if (dict != null) petCount = dict.Count;
                }

                if (!string.IsNullOrEmpty(requestsJson) && requestsJson != "null")
                {
                    var dict = MiniJSON.Json.Deserialize(requestsJson) as Dictionary<string, object>;
                    if (dict != null) requestCount = dict.Count;
                }

                int totalCount = petCount + requestCount;

                if (totalCount >= MaxPets)
                {
                    AddViewerToQueue();
                }
                else
                {
                    AssignRandomPet();
                    spawnScreen.SetActive(true);
                    feedbackText.text = "";
                    nameInput.text = "";
                    feedbackBox.SetActive(false);
                }
            });
        });
}

    /// <summary>
    /// Adds viewer to Firebase queue and starts monitoring queue status and position.
    /// </summary>
    void AddViewerToQueue()
    {
        isInQueue = true;
        string json = $"{{\"status\":\"waiting\"}}";
        FirebaseREST.Instance.SetData($"ecosystem/queue/{viewerID}", json);

        welcomeScreen.SetActive(false);
        queueScreen.SetActive(true);
        spawnScreen.SetActive(false);
        successScreen.SetActive(false);

        StartCoroutine(PollQueueStatus());
        StartCoroutine(UpdateQueuePositionRealtime());
    }

    /// <summary>
    /// Monitors viewer's queue entry for status changes.
    /// Moves to spawn screen when marked as ready.
    /// </summary>
    IEnumerator PollQueueStatus()
    {
        while (isInQueue)
        {
            FirebaseREST.Instance.GetData($"ecosystem/queue/{viewerID}", json =>
            {
                if (!string.IsNullOrEmpty(json) && json.Contains("ready"))
                {
                    spawnScreen.SetActive(true);
                    AssignRandomPet();
                    feedbackText.text = "";
                    nameInput.text = "";
                    feedbackBox.SetActive(false);
                    queueScreen.SetActive(false);

                    FirebaseREST.Instance.DeleteData($"ecosystem/queue/{viewerID}");
                    isInQueue = false;
                }
            });

            yield return new WaitForSeconds(2f);
        }
    }

    /// <summary>
    /// Updates displayed queue position in real time by counting entries before viewer.
    /// </summary>
    IEnumerator UpdateQueuePositionRealtime()
    {
        while (isInQueue)
        {
            FirebaseREST.Instance.GetData("ecosystem/queue", json =>
            {
                int position = 1;
                if (!string.IsNullOrEmpty(json) && json != "null")
                {
                    var dict = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;
                    foreach (var kv in dict.Keys)
                    {
                        if (kv == viewerID) break;
                        position++;
                    }
                }
                queuePositionText.text = "Position: " + position;
            });

            yield return new WaitForSeconds(3f);
        }
    }

    /// <summary>
    /// Selects a pet type based on weighted probabilities and updates preview animator.
    /// Hostile pets have lower chance of being picked.
    /// </summary>
    void AssignRandomPet()
    {
        // 80% peaceful pet, 20% hostile pet
        float passivePetProbability = 0.95f;
        string[] passivePets = { "Cat", "Dog", "Deer" };
        string[] aggressivePets = { "Wolf", "Bear", "Tiger" };
        string[] selectedGroup;
        
        // Pick a group
        float groupRoll = Random.value;
        if (groupRoll < passivePetProbability)
        {
            selectedGroup = passivePets;
        }
        else
        {
            selectedGroup = aggressivePets;
        }

        // Inside the picked group select a random pet
        assignedPet = selectedGroup[Random.Range(0, selectedGroup.Length)];

        switch (assignedPet)
        {
            case "Cat": petPreviewAnimator.runtimeAnimatorController = catAnimator; break;
            case "Dog": petPreviewAnimator.runtimeAnimatorController = dogAnimator; break;
            case "Wolf": petPreviewAnimator.runtimeAnimatorController = wolfAnimator; break;
            case "Deer": petPreviewAnimator.runtimeAnimatorController = deerAnimator; break;
            case "Bear": petPreviewAnimator.runtimeAnimatorController = bearAnimator; break;
            case "Tiger": petPreviewAnimator.runtimeAnimatorController = tigerAnimator; break;
            /*
            case "Pikachu": petPreviewAnimator.runtimeAnimatorController = pikachuAnimator; break;
            case "Bulbasaur": petPreviewAnimator.runtimeAnimatorController = bulbasaurAnimator; break;
            case "Charizard": petPreviewAnimator.runtimeAnimatorController = charizardAnimator; break;
            case "Kabuto": petPreviewAnimator.runtimeAnimatorController = kabutoAnimator; break;
            case "Squirtle": petPreviewAnimator.runtimeAnimatorController = squirtleAnimator; break;
            */
        }
        Debug.Log($"Assigned pet: {assignedPet}");
    }

    void CheckCapacityBeforeSpawn(string petName)
    {
        FirebaseREST.Instance.GetData("ecosystem/pets", petsJson =>
        {
            FirebaseREST.Instance.GetData("ecosystem/spawnRequests", requestsJson =>
            {
                int petCount = 0;
                int requestCount = 0;

                if (!string.IsNullOrEmpty(petsJson) && petsJson != "null")
                {
                    var dict = MiniJSON.Json.Deserialize(petsJson) as Dictionary<string, object>;
                    if (dict != null) petCount = dict.Count;
                }

                if (!string.IsNullOrEmpty(requestsJson) && requestsJson != "null")
                {
                    var dict = MiniJSON.Json.Deserialize(requestsJson) as Dictionary<string, object>;
                    if (dict != null) requestCount = dict.Count;
                }

                int totalCount = petCount + requestCount;

                if (totalCount >= MaxPets)
                {
                    // Send back to queue if ecosystem filled up
                    AddViewerToQueue();
                    return;
                }

                // Push spawn request
                string json = $"{{\"name\":\"{petName}\",\"type\":\"{assignedPet}\"}}";
                FirebaseREST.Instance.PushData("ecosystem/spawnRequests", json);

                GoToSuccessScreen();
            });
        });
    }
}
