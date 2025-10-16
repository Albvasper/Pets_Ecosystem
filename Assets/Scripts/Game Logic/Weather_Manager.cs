using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Weather_Manager : MonoBehaviour
{
    public static Weather_Manager Instance { get; private set; }

    public event Action<bool> OnWeatherChanged;
    
    [SerializeField] GameObject RainGameObject;
    [SerializeField] Light2D Light2D;
    const float RainLightIntensity = 0.57f;
    const float NormalLightIntensity = 1f;
    float minRainInterval = 300f; // 5 min
    float maxRainInterval = 600f; // 10 min
    float rainDuration = 300f; /// 5 min
    float rainTimer;
    bool raining;


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
        rainTimer = 0;
    }

    void Update()
    {
        rainTimer -= Time.deltaTime;
        if (rainTimer <= 0f)
        {
            raining = !raining;
            if (raining)
            {
                StartRaining();
                rainTimer = rainDuration;
            }
            else
            {
                StopRaining();
                ScheduleNextRain();
            }
        }
    }

    void ScheduleNextRain()
    {
        rainTimer = UnityEngine.Random.Range(minRainInterval, maxRainInterval);
    }
    
    void StartRaining()
    {
        raining = true;
        RainGameObject.SetActive(true);
        Light2D.intensity = RainLightIntensity;
        OnWeatherChanged?.Invoke(raining);
    }
    
    void StopRaining()
    {
        raining = false;
        RainGameObject.SetActive(false);
        Light2D.intensity = NormalLightIntensity;
        OnWeatherChanged?.Invoke(raining);
    }
}
