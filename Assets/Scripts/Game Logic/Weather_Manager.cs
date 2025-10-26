using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class Weather_Manager : MonoBehaviour
{
    public static Weather_Manager Instance { get; private set; }

    public event Action OnWeatherChanged;
    
    [SerializeField] GameObject RainGameObject;
    [SerializeField] Light2D Light2D;
    [SerializeField] GameObject OverWorld;
    [SerializeField] GameObject SnowyWorld;

    [Header("Rain Event Settings")]
    const float RainLightIntensity = 0.57f;
    const float NormalLightIntensity = 1f;
    float minRainInterval = 300f; // 5 min
    float maxRainInterval = 600f; // 10 min
    float rainDuration = 300f; /// 5 min
    float rainTimer;
    public bool Raining { get; set; }
    [Header("Snow Event Settings")]
    [SerializeField] float minTimeBetweenSnow = 600f; // 10 minutes
    [SerializeField] float maxTimeBetweenSnow = 1200f; // 20 minutes
    [SerializeField] float minSnowDuration = 180f; // 3 minutes
    [SerializeField] float maxSnowDuration = 300f; // 5 minutes
    [SerializeField] float snowEventChance = 0.3f; // 30% chance when timer triggers
    public bool Snowing { get; set; }

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
