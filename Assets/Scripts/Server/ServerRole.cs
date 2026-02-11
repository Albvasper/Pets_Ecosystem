using UnityEngine;

public class ServerRole : MonoBehaviour
{
    public static bool IsServer;
    void Awake() => IsServer = true;
}
