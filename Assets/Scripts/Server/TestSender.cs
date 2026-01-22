using UnityEngine;
using NativeWebSocket;

public class TestSender : MonoBehaviour
{
    WebSocket ws;

    async void Start()
    {
        ws = new WebSocket("wss://ws-server-production-02ef.up.railway.app");

        ws.OnOpen += () =>
        {
            Debug.Log("ECOSYSTEM WS CONNECTED");
            InvokeRepeating(nameof(SendTest), 1f, 2f);
        };

        ws.OnError += e => Debug.LogError("WS ERROR: " + e);

        await ws.Connect();
    }

    async void SendTest()
    {
        if (ws.State == WebSocketState.Open)
        {
            Debug.Log("SENDING TEST MESSAGE");
            await ws.SendText("hello from ecosystem");
        }
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        ws?.DispatchMessageQueue();
#endif
    }
}
