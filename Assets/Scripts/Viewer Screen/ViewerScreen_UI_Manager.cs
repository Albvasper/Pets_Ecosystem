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

        // Push spawn request
        string json = $"{{\"name\":\"{petName}\",\"type\":\"{assignedPet}\"}}";
        FirebaseREST.Instance.PushData("ecosystem/spawnRequests", json);

        GoToSuccessScreen();
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
        FirebaseREST.Instance.GetData("ecosystem/pets", json =>
        {
            int petCount = 0;
            if (!string.IsNullOrEmpty(json) && json != "null")
            {
                var dict = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;
                if (dict != null) petCount = dict.Count;
            }

            if (petCount < MaxPets)
            {
                AssignRandomPet();
                spawnScreen.SetActive(true);
                feedbackText.text = "";
                nameInput.text = "";
                feedbackBox.SetActive(false);
            }
            else
            {
                AddViewerToQueue();
            }
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

        queueScreen.SetActive(true);
        spawnScreen.SetActive(false);

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
    /// Randomly selects a pet type and updates preview animator.
    /// </summary>
    void AssignRandomPet()
    {
        string[] petTypes = { "Cat", "Dog", "Wolf", "Deer", "Tiger", "Bear" };
        assignedPet = petTypes[Random.Range(0, petTypes.Length)];

        switch (assignedPet)
        {
            case "Cat": petPreviewAnimator.runtimeAnimatorController = catAnimator; break;
            case "Dog": petPreviewAnimator.runtimeAnimatorController = dogAnimator; break;
            case "Wolf": petPreviewAnimator.runtimeAnimatorController = wolfAnimator; break;
            case "Deer": petPreviewAnimator.runtimeAnimatorController = deerAnimator; break;
            case "Bear": petPreviewAnimator.runtimeAnimatorController = bearAnimator; break;
            case "Tiger": petPreviewAnimator.runtimeAnimatorController = tigerAnimator; break;
        }

        Debug.Log($"Assigned pet: {assignedPet}");
    }
}
