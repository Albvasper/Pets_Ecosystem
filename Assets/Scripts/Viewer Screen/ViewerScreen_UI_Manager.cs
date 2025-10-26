using System.Collections;
using UnityEngine;
using TMPro;
using Firebase.Database;
using Firebase.Extensions;

public class ViewerScreenUIManager : MonoBehaviour
{
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

    private DatabaseReference db;
    private string viewerID;
    private string assignedPet;
    private bool isInQueue = false;

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
        StartCoroutine(WaitForFirebase());
    }

    IEnumerator WaitForFirebase()
    {
        while (!FirebaseInit.Ready) yield return null;
        db = FirebaseInit.db;

        ListenForPetCountChanges();
    }

    void ListenForPetCountChanges()
    {
        db.Child("ecosystem").Child("pets").ValueChanged += (sender, args) =>
        {
            if (args.Snapshot == null) return;

            int currentPetCount = (int)args.Snapshot.ChildrenCount;

            if (currentPetCount < 20 && isInQueue)
            {
                spawnScreen.SetActive(true);
                AssignRandomPet();
                feedbackBox.SetActive(false);
                feedbackText.text = "";
                nameInput.text = "";
                queueScreen.SetActive(false);
                isInQueue = false;

                db.Child("ecosystem").Child("queue").Child(viewerID).RemoveValueAsync();
            }
        };
    }

    // -------------------------------
    // CONNECT TO ECOSYSTEM
    // -------------------------------
    public void ConnectToEcosystem()
    {
        if (db == null) return;
        if (string.IsNullOrEmpty(publicKeyInput.text))
        {
            publicKeyFeedbackText.text = "Introduce a key!";
            return;
        }
        else
        {
            welcomeScreen.SetActive(false);
            queueScreen.SetActive(false);
            spawnScreen.SetActive(false);
            successScreen.SetActive(false);
            CheckEcosystemCapacity();
        }
    }

    void CheckEcosystemCapacity()
    {
        db.Child("ecosystem").Child("pets").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.Result == null) return;

            int petCount = (int)task.Result.ChildrenCount;

            if (petCount < 20)
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

    // -------------------------------
    // QUEUE SYSTEM
    // -------------------------------
    void AddViewerToQueue()
    {
        if (db == null) return;

        isInQueue = true;
        var viewerRef = db.Child("ecosystem").Child("queue").Child(viewerID);
        viewerRef.Child("status").SetValueAsync("waiting");

        queueScreen.SetActive(true);
        spawnScreen.SetActive(false);

        viewerRef.Child("status").ValueChanged += (sender, args) =>
        {
            if (args.Snapshot.Exists && args.Snapshot.Value.ToString() == "ready")
            {
                spawnScreen.SetActive(true);
                AssignRandomPet();
                feedbackText.text = "";
                nameInput.text = "";
                feedbackBox.SetActive(false);
                queueScreen.SetActive(false);
                viewerRef.RemoveValueAsync();
                isInQueue = false;
            }
        };

        UpdateQueuePosition();
        StartCoroutine(UpdateQueuePositionRealtime());
    }

    IEnumerator UpdateQueuePositionRealtime()
    {
        while (isInQueue)
        {
            UpdateQueuePosition();
            yield return new WaitForSeconds(3f);
        }
    }

    void UpdateQueuePosition()
    {
        db.Child("ecosystem").Child("queue").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || !task.IsCompleted) return;

            int position = 1;
            foreach (var child in task.Result.Children)
            {
                if (child.Key == viewerID) break;
                position++;
            }
            queuePositionText.text = "Position: " + position;
        });
    }

    public void CancelQueue()
    {
        welcomeScreen.SetActive(true);
        queueScreen.SetActive(false);
        spawnScreen.SetActive(false);
        successScreen.SetActive(false);

        isInQueue = false;
        if (db != null)
            db.Child("ecosystem").Child("queue").Child(viewerID).RemoveValueAsync();
    }

    // -------------------------------
    // PET ASSIGNMENT
    // -------------------------------
    void AssignRandomPet()
    {
        string[] petTypes = { "Cat", "Dog", "Wolf", "Deer", "Tiger", "Bear" };
        assignedPet = petTypes[Random.Range(0, petTypes.Length)];

        switch (assignedPet)
        {
            case "Cat":
                petPreviewAnimator.runtimeAnimatorController = catAnimator;
                break;
            case "Dog":
                petPreviewAnimator.runtimeAnimatorController = dogAnimator;
                break;
            case "Wolf":
                petPreviewAnimator.runtimeAnimatorController = wolfAnimator;
                break;
            case "Deer":
                petPreviewAnimator.runtimeAnimatorController = deerAnimator;
                break;
            case "Bear":
                petPreviewAnimator.runtimeAnimatorController = bearAnimator;
                break;
            case "Tiger":
                petPreviewAnimator.runtimeAnimatorController = tigerAnimator;
                break;
        }

        Debug.Log($"Assigned pet: {assignedPet}");
    }

    // -------------------------------
    // SPAWN PET
    // -------------------------------
    public void NameAndSpawnPet()
    {
        if (db == null) return;
        string petName = nameInput.text.Trim();
        if (string.IsNullOrEmpty(petName))
        {
            feedbackBox.SetActive(true);
            feedbackText.text = "Enter a name!";
            return;
        }

        db.Child("ecosystem").Child("spawnRequests").Push().SetRawJsonValueAsync(
            $"{{\"name\":\"{petName}\",\"type\":\"{assignedPet}\"}}"
        ).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                GoToSuccessScreen();
            }
            else
            {
                feedbackBox.SetActive(true);
                feedbackText.text = "Failed to send request!";
            }
        });
    }

    public void GoToSuccessScreen()
    {
        welcomeScreen.SetActive(false);
        queueScreen.SetActive(false);
        spawnScreen.SetActive(false);
        successScreen.SetActive(true);
    }
}
