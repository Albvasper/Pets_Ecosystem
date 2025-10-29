using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Text;

/// <summary>
/// Manages Firebase Realtime Database operations including GET, SET, PUSH, and DELETE requests.
/// </summary>
public class FirebaseInit : MonoBehaviour
{
    public static FirebaseInit Instance;
    /// <summary>
    /// Indicates if firebase is ready to be used or not.
    /// </summary>
    public static bool Ready = false;
    /// <summary>
    /// Firebase database URL.
    /// </summary>
    public static string baseURL = "https://pet-ecosystem-default-rtdb.firebaseio.com/";

    void Awake()
    {
        Instance = this;
        Ready = true;
        Debug.Log("Firebase REST initialized for WebGL");
    }

    /// <summary>
    /// Retrieves a value from Firebase Realtime Database at the specified path.
    /// </summary>
    /// <param name="path">Database path relative to base URL.</param>
    /// <param name="onComplete">Callback invoked with JSON string response on successful retrieval.</param>
    /// <returns>Coroutine that performs the asynchronous GET request.</returns>
    public IEnumerator GetValue(string path, System.Action<string> onComplete)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(baseURL + path + ".json"))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
                onComplete?.Invoke(request.downloadHandler.text);
            else
                Debug.LogError("GET failed: " + request.error);
        }
    }

    /// <summary>
    /// Sets a value in Firebase Realtime Database at the specified path.
    /// Overwrites existing data at the path with the provided JSON data.
    /// </summary>
    /// <param name="path">Database path relative to base URL.</param>
    /// <param name="jsonData">JSON string to store at the specified path.</param>
    /// <param name="onComplete">Optional callback invoked with true if successful, false if failed.</param>
    /// <returns>Coroutine that performs the asynchronous PUT request.</returns>
    public IEnumerator SetValue(string path, string jsonData, System.Action<bool> onComplete = null)
    {
        using (UnityWebRequest request = new UnityWebRequest(baseURL + path + ".json", "PUT"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            onComplete?.Invoke(request.result == UnityWebRequest.Result.Success);
        }
    }

    /// <summary>
    /// Pushes a new value to Firebase Realtime Database, generating a unique key.
    /// Creates a new child entry under the specified path with an auto generated ID.
    /// </summary>
    /// <param name="path">Database path where the new entry will be added.</param>
    /// <param name="jsonData">JSON string to store as a new entry.</param>
    /// <param name="onComplete">Optional callback invoked with true if successful, false if failed.</param>
    /// <returns>Coroutine that performs the asynchronous POST request.</returns>
    public IEnumerator PushValue(string path, string jsonData, System.Action<bool> onComplete = null)
    {
        using (UnityWebRequest request = new UnityWebRequest(baseURL + path + ".json", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            onComplete?.Invoke(request.result == UnityWebRequest.Result.Success);
        }
    }

    /// <summary>
    /// Deletes a value from Firebase Realtime Database at the specified path.
    /// Removes the data and all its children at the given path.
    /// </summary>
    /// <param name="path">Database path to delete.</param>
    /// <param name="onComplete">Optional callback invoked with true if successful, false if failed.</param>
    /// <returns>Coroutine that performs the asynchronous DELETE request.</returns>
    public IEnumerator DeleteValue(string path, System.Action<bool> onComplete = null)
    {
        using (UnityWebRequest request = UnityWebRequest.Delete(baseURL + path + ".json"))
        {
            yield return request.SendWebRequest();
            onComplete?.Invoke(request.result == UnityWebRequest.Result.Success);
        }
    }
}
