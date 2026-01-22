using UnityEngine;
using NativeWebSocket;

public class WebSocketClient : MonoBehaviour
{
    WebSocket ws;

    async void Start()
    {
        ws = new WebSocket("wss://ws-server-production-02ef.up.railway.app");

        ws.OnMessage += (bytes) =>
        {
            string json = System.Text.Encoding.UTF8.GetString(bytes);

            Debug.Log("RAW SNAPSHOT: " + json);

            WorldSnapshot snapshot = JsonUtility.FromJson<WorldSnapshot>(json);

            Debug.Log("APPLYING SNAPSHOT");

            SnapshotReceiver.Apply(snapshot);
        };

        await ws.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        ws?.DispatchMessageQueue();
#endif
    }

    async void OnApplicationQuit()
    {
        if (ws != null)
            await ws.Close();
    }
}
