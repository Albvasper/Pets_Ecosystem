using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] public GameObject RainGameObject;
    [SerializeField] public Light2D Light2D;
    bool isRaining;
    const float RainLightIntensity = 0.57f;
    const float NormalLightIntensity = 1f;
    public static Player Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        RainGameObject.SetActive(false);
        isRaining = false;
    }

    public void ToggleRain()
    {
        if (!isRaining)
        {
            isRaining = true;
            RainGameObject.SetActive(true);
            Light2D.intensity = RainLightIntensity;
        }
        else
        {
            isRaining = false;
            RainGameObject.SetActive(false);
            Light2D.intensity = NormalLightIntensity;
        }
    }
}
