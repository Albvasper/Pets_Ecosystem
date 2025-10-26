using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class FirebaseInit : MonoBehaviour
{
    public static bool Ready = false;
    public static DatabaseReference db;

    void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Get database instance
                var database = FirebaseDatabase.GetInstance(FirebaseApp.DefaultInstance);

                // Disable offline persistence to allow multiple Unity instances
                database.SetPersistenceEnabled(false);

                // Get root reference
                db = database.RootReference;

                Ready = true;
                Debug.Log("Firebase DB ready!");
            }
            else
            {
                Debug.LogError("Could not resolve Firebase dependencies: " + dependencyStatus);
            }
        });
    }
}
