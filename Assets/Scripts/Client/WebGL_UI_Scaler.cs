using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class WebGL_UI_Scaler : MonoBehaviour
{
    CanvasScaler scaler;

    void Awake()
    {
        scaler = GetComponent<CanvasScaler>();
    }

    void Update()
    {
        var scaler = GetComponent<CanvasScaler>();
        float currentAspect = (float)Screen.width / Screen.height;
        float targetAspect = 16f / 9f;

        if (currentAspect < targetAspect)
            scaler.scaleFactor = 1f;
        else
            scaler.scaleFactor = Screen.height / 1080f * 0.95f; 
    }
}