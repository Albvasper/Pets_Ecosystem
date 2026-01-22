using UnityEngine;
using NativeWebSocket;

public class EcosystemWebSocketClient : MonoBehaviour
{
    public static EcosystemWebSocketClient Instance;

    WebSocket ws;

    async void Awake()
    {
        Instance = this;

        ws = new WebSocket("wss://ws-server-production-02ef.up.railway.app");

        ws.OnOpen += () =>
        {
            Debug.Log("ECOSYSTEM CONNECTED TO WS");
        };

        ws.OnError += e =>
        {
            Debug.LogError("ECOSYSTEM WS ERROR: " + e);
        };

        await ws.Connect();
    }

    public async void Send(string msg)
    {
        if (ws != null && ws.State == WebSocketState.Open)
        {
            await ws.SendText(msg);
        }
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        ws?.DispatchMessageQueue();
#endif
    }
}
