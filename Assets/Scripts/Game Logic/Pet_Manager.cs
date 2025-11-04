using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

/// <summary>
/// Tracks every pet on the ecosystem and passes updated data to
/// the UI manager to display it properly.
/// </summary>
public class Pet_Manager : MonoBehaviour
{
    public static Pet_Manager Instance { get; private set; }
    /// <summary>
    /// Collection of all the pets that are alive on the ecosystem currently.
    /// </summary>
    public List<BaseAnimal> Pets { get; private set; }
    /// <summary>
    /// Limit of pets on the ecosystem
    /// </summary>
    public int maxPets = 20;
    /// <summary>
    /// Indicates a change on the population.
    /// </summary>
    public event Action OnPopulationChanged;
    /// <summary>
    /// Indicates a change on the population happiness.
    /// </summary>
    public event Action OnHappinessChanged;
    /// <summary>
    /// Indicates a change on the sentience of the population.
    /// </summary>
    public event Action OnSentienceChanged;
    /// <summary>
    /// Indicates that a new birth occured.
    /// </summary>
    public event Action OnBirthRateChanged;
    public int Population { get; private set; }
    public int PopulationDogs { get; private set; }
    public int PopulationCats { get; private set; }
    public int PopulationDeers { get; private set; }
    public int PopulationWolves { get; private set; }
    public int PopulationTigers { get; private set; }
    public int PopulationBears { get; private set; }
    public int PopulationZombies { get; private set; }
    public float BirthRate { get; private set; }
    float birthTimer = 0f;
    int birthsInWindow = 0;

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
        Pets = new List<BaseAnimal>();
        Population = 0;
        PopulationDogs = 0;
        PopulationCats = 0;
        PopulationDeers = 0;
        PopulationWolves = 0;
        PopulationTigers = 0;
        PopulationBears = 0;
        BirthRate = 0;
        PopulationZombies = 0;
    }

    void Update()
    {
        // Measure the birth rate with birthInWindow.
        birthTimer += Time.deltaTime;
        if (birthTimer >= 60f)
        {
            BirthRate = birthsInWindow;
            birthsInWindow = 0;
            birthTimer = 0f;
            OnBirthRateChanged?.Invoke();
        }
    }

    public void AddToPopulation()
    {
        Population++;
        OnPopulationChanged?.Invoke();
    }

    public void RemoveFromPopulation()
    {
        Population--;
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

    public void AddToTigerPopulation()
    {
        PopulationTigers++;
        AddToPopulation();
    }

    public void AddToBearPopulation()
    {
        PopulationBears++;
        AddToPopulation();
    }

    public void AddToZombiePopulation()
    {
        PopulationZombies++;
        OnPopulationChanged?.Invoke();
    }

    public void RemoveFromDogPopulation()
    {
        PopulationDogs--;
        RemoveFromPopulation();
    }

    public void RemoveFromCatPopulation()
    {
        PopulationCats--;
        RemoveFromPopulation();
    }

    public void RemoveFromDeerPopulation()
    {
        PopulationDeers--;
        RemoveFromPopulation();
    }

    public void RemoveFromWolfPopulation()
    {
        PopulationWolves--;
        RemoveFromPopulation();
    }

    public void RemoveFromTigerPopulation()
    {
        PopulationTigers--;
        RemoveFromPopulation();
    }

    public void RemoveFromBearPopulation()
    {
        PopulationBears--;
        RemoveFromPopulation();
    }

    public void RemoveFromZombiePopulation()
    {
        PopulationZombies--;
        OnPopulationChanged?.Invoke();
    }

    public void NotifyHappinessChanged()
    {
        OnHappinessChanged?.Invoke();
    }

    public void NotifySentienceChanged()
    {
        OnSentienceChanged?.Invoke();
    }

    public float GetAverageHappiness()
    {
        if (Pets.Count == 0)
            return 0;
        float total = 0;
        foreach (BaseAnimal pet in Pets)
            total += pet.Happiness;
        return total / Pets.Count;
    }

    public float GetAverageSentience()
    {
        if (Pets.Count == 0)
            return 0;
        float total = 0;
        foreach (BaseAnimal pet in Pets)
            total += pet.Sentience;
        return total / Pets.Count;
    }

    public void RegisterBirth()
    {
        birthsInWindow++;
    }

    public bool HasFreeSpace()
    {
        return Pets.Count < maxPets;
    }

    public void KillAllPets()
    {
        for (int i = Pets.Count - 1; i >= 0; i--)
        {
            Pets[i].Die();
        }
    }
}
