using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    [SerializeField] public GameObject RainGameObject;
    [SerializeField] public Light2D Light2D;
    bool isRaining;
    const float RainLightIntensity = 0.57f;
    const float NormalLightIntensity = 1f;
    public static Player Instance { get; private set; }

    // Stats
    public event Action OnPopulationChanged;

    public int Happiness { get; set; }
    public int Sentience { get; set; }
    public int Population { get; private set; }
    public int PopulationDogs { get; private set; }
    public int PopulationCats { get; private set; }
    public int PopulationDeers { get; private set; }
    public int PopulationWolves { get; private set; }
    public int PopulationZombies { get; private set; }
    public int BirthRate { get; set; }
    public int Time { get; set; }
    public int Weather { get; set; }


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
        Happiness = 0;
        Sentience = 0;
        Population = 0;
        PopulationDogs = 0;
        PopulationCats = 0;
        PopulationDeers = 0;
        PopulationWolves = 0;
        BirthRate = 0;
        PopulationZombies = 0;
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

    public void AddToPopulation()
    {
        Population++;
        OnPopulationChanged?.Invoke();
    }

    public void AddToDogPopulation()
    {
        PopulationDogs++;
        AddToPopulation();
    }

    public void AddToCatPopulation()
    {
        PopulationCats++;
        AddToPopulation();
    }

    public void AddToDeerPopulation()
    {
        PopulationDeers++;
        AddToPopulation();
    }

    public void AddToWolfPopulation()
    {
        PopulationWolves++;
        AddToPopulation();
    }

    public void AddToZombiePopulation()
    {
        PopulationZombies++;
        AddToPopulation();
    }
}
