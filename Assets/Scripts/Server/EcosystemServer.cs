using UnityEngine;

public class EcosystemServer : MonoBehaviour
{
    void Awake()
    {
#if UNITY_SERVER || !UNITY_WEBGL
        Application.targetFrameRate = 30;
#else
        Destroy(gameObject);
#endif
    }
}
