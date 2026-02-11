using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Updated current weather on client side.
/// </summary>
public class ClientEcosystemWeatherManager : MonoBehaviour
{
    public static ClientEcosystemWeatherManager Instance { get; private set; }

    [SerializeField] GameObject RainGameObject;
    [SerializeField] Light2D Light2D;
    [SerializeField] GameObject OverWorld;
    [SerializeField] GameObject SnowyWorld;

    const float RainLightIntensity = 0.57f;
    const float NormalLightIntensity = 1f;

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
    }

    public void SetWeatherRain()
    {
        Light2D.intensity = RainLightIntensity;
        OverWorld.SetActive(true);
        RainGameObject.SetActive(true);
        SnowyWorld.SetActive(false);
    }

    public void SetWeatherSnowy()
    {
        Light2D.intensity = NormalLightIntensity;
        OverWorld.SetActive(false);
        SnowyWorld.SetActive(true);
        RainGameObject.SetActive(false);
    }

    public void SetWeatherSunny()
    {
        Light2D.intensity = NormalLightIntensity;
        OverWorld.SetActive(true);
        SnowyWorld.SetActive(false);
        RainGameObject.SetActive(false);
    }
}
