using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

/// <summary>
/// Responsible for changing current weather (Sunny, raining or snowing).
/// </summary>
public class Weather_Manager : MonoBehaviour
{
    public static Weather_Manager Instance { get; private set; }
    /// <summary>
    /// Indicates that weather has changed and needs an update.
    /// </summary>
    public event Action OnWeatherChanged;
    [SerializeField] GameObject RainGameObject;
    [SerializeField] Light2D Light2D;
    [SerializeField] GameObject OverWorld;
    [SerializeField] GameObject SnowyWorld;
    [Header("Rain Event Settings")]
    public bool Raining { get; set; }
    [Header("Snow Event Settings")]
    public bool Snowing { get; set; }
    const float RainLightIntensity = 0.57f;
    const float NormalLightIntensity = 1f;
    float minRainInterval = 300f;           // Set to 5 min.
    float maxRainInterval = 600f;           // Set to 10 min.
    float rainDuration = 300f;              // Set to 5 min.
    float rainTimer;
    float minTimeBetweenSnow = 600f;        // Set to 10 min.
    float maxTimeBetweenSnow = 1200f;       // Set to 20 min.
    float minSnowDuration = 180f;           // Set to 3 min.
    float maxSnowDuration = 300f;           // Set to 5 min.
    float snowEventChance = 0.3f;           // 30% chance of snowing.
    
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

    void Start()
    {
        StartCoroutine(SnowCycle());
    }

    void Update()
    {
        rainTimer -= Time.deltaTime;
        if (rainTimer <= 0f)
        {
            Raining = !Raining;
            if (Raining)
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
        Raining = true;
        RainGameObject.SetActive(true);
        Light2D.intensity = RainLightIntensity;
        OnWeatherChanged?.Invoke();
    }

    void StopRaining()
    {
        Raining = false;
        RainGameObject.SetActive(false);
        Light2D.intensity = NormalLightIntensity;
        OnWeatherChanged?.Invoke();
    }

    IEnumerator SnowCycle()
    {
        while (true)
        {
            float waitTime = UnityEngine.Random.Range(minTimeBetweenSnow, maxTimeBetweenSnow);
            yield return new WaitForSeconds(waitTime);
            // Random chance for snow event
            if (UnityEngine.Random.value <= snowEventChance)
            {
                yield return StartCoroutine(TriggerSnowEvent());
            }
        }
    }

    IEnumerator TriggerSnowEvent()
    {
        if (Snowing) yield break;
        Snowing = true;
        SetSnowyWorld();
        // Keep it snowy for random duration
        float snowDuration = UnityEngine.Random.Range(minSnowDuration, maxSnowDuration);
        yield return new WaitForSeconds(snowDuration);
        SetNormalWorld();
        Snowing = false;
    }

    void SetNormalWorld()
    {
        OnWeatherChanged?.Invoke();
        OverWorld.SetActive(true);
        SnowyWorld.SetActive(false);
    }

    void SetSnowyWorld()
    {
        OnWeatherChanged?.Invoke();
        OverWorld.SetActive(false);
        SnowyWorld.SetActive(true);
    }

    [ContextMenu("Trigger Snow Event")]
    public void ManualSnowTrigger()
    {
        if (!Snowing)
        {
            StartCoroutine(TriggerSnowEvent());
        }
    }
}
