using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Manages Firebase Realtime Database REST API operations.
/// </summary>
public class FirebaseREST : MonoBehaviour
{
    public static FirebaseREST Instance;
    /// <summary>
    /// Firebase Realtime Database URL.
    /// </summary>
    public string databaseURL = "https://pet-ecosystem-default-rtdb.firebaseio.com/";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    /// <summary>
    /// Writes data at the specified path in Firebase.
    /// Uses PUT method to replace existing data.
    /// </summary>
    /// <param name="path">Database path relative to base URL.</param>
    /// <param name="json">JSON string to store at the path.</param>
    public void SetData(string path, string json)
    {
        StartCoroutine(SetDataCoroutine(path, json));
    }

    IEnumerator SetDataCoroutine(string path, string json)
    {
        string url = $"{databaseURL}/{path}.json";
        using (UnityWebRequest req = new UnityWebRequest(url, "PUT"))
        {
            byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(body);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
                Debug.Log($"Firebase: Wrote data to {path}");
            else
                Debug.LogError($"Firebase write error ({path}): {req.error}");
        }
    }

    /// <summary>
    /// Pushes new data to database, generating a unique auto generated key.
    /// Creates a new child entry under the specified path.
    /// </summary>
    /// <param name="path">Database path where new entry will be added.</param>
    /// <param name="json">JSON string to store as a new entry.</param>
    public void PushData(string path, string json)
    {
        StartCoroutine(PushDataCoroutine(path, json));
    }

    private IEnumerator PushDataCoroutine(string path, string json)
    {
        string url = $"{databaseURL}/{path}.json";
        using (UnityWebRequest req = new UnityWebRequest(url, "POST"))
        {
            byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(body);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Firebase: Pushed data to {path}");
                Debug.Log($"Response: {req.downloadHandler.text}");
            }
            else
            {
                Debug.LogError($"Firebase push error ({path}): {req.error}");
            }
        }
    }

    /// <summary>
    /// Retrieves data from database at the specified path.
    /// </summary>
    /// <param name="path">Database path to read.</param>
    /// <param name="callback">Callback invoked with JSON string response on success.</param>
    public void GetData(string path, Action<string> callback)
    {
        StartCoroutine(GetDataCoroutine(path, callback));
    }

    IEnumerator GetDataCoroutine(string path, Action<string> callback)
    {
        string url = $"{databaseURL}/{path}.json";
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
                callback?.Invoke(req.downloadHandler.text);
            else
                Debug.LogError($"Firebase read error ({path}): {req.error}");
        }
    }

    /// <summary>
    /// Deletes data from database at the specified path.
    /// Removes the data and all its children permanently.
    /// </summary>
    /// <param name="path">Database path to delete.</param>
    /// <param name="onComplete">Optional callback invoked with true if successful, false if failed.</param>
    public void DeleteData(string path, System.Action<bool> onComplete = null)
    {
        StartCoroutine(DeleteDataCoroutine(path, onComplete));
    }

    private IEnumerator DeleteDataCoroutine(string path, System.Action<bool> onComplete)
    {
        string url = $"{databaseURL}/{path}.json";

        using (UnityWebRequest request = UnityWebRequest.Delete(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"[FirebaseREST] Deleted data at: {path}");
                onComplete?.Invoke(true);
            }
            else
            {
                Debug.LogError($"[FirebaseREST] Failed to delete {path}: {request.error}");
                onComplete?.Invoke(false);
            }
        }
    }
}
