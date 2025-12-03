using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates the UI with fresh and accurate data. 
/// Listens to changes and displays it on the UI.
/// </summary>
public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance { get; private set; }
    public Image HappinessBar;
    public Image SentienceBar;
    [SerializeField] private TextMeshProUGUI TextPopulation;
    [SerializeField] private TextMeshProUGUI TextPopulationDogs;
    [SerializeField] private TextMeshProUGUI TextPopulationCats;
    [SerializeField] private TextMeshProUGUI TextPopulationDeers;
    [SerializeField] private TextMeshProUGUI TextPopulationWolves;
    [SerializeField] private TextMeshProUGUI TextPopulationTigers;
    [SerializeField] private TextMeshProUGUI TextPopulationBears;
    [SerializeField] private TextMeshProUGUI TextPopulationZombies;
    [SerializeField] private TextMeshProUGUI TextPopulationPokemons;
    [SerializeField] private TextMeshProUGUI TextBirthRate;
    [SerializeField] private TextMeshProUGUI TextWeather;

    public int Time { get; set; }

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

    void OnEnable()
    {
        Pet_Manager.Instance.OnPopulationChanged += UpdatePopulation;
        Pet_Manager.Instance.OnHappinessChanged += UpdateHappinessBar;
        Pet_Manager.Instance.OnSentienceChanged += UpdateSentienceBar;
        Pet_Manager.Instance.OnBirthRateChanged += UpdateBirthRate;
        Weather_Manager.Instance.OnWeatherChanged += UpdateWeatherUI;
        UpdateHappinessBar();
    }

    void OnDisable()
    {
        Pet_Manager.Instance.OnPopulationChanged -= UpdatePopulation;
        Pet_Manager.Instance.OnHappinessChanged -= UpdateHappinessBar;
        Pet_Manager.Instance.OnSentienceChanged -= UpdateSentienceBar;
        Pet_Manager.Instance.OnBirthRateChanged -= UpdateBirthRate;
        Weather_Manager.Instance.OnWeatherChanged -= UpdateWeatherUI;
    }

    void Start()
    {
        UpdatePopulation();
        UpdateHappinessBar();
        UpdateSentienceBar();
        UpdateBirthRate();
    }

    void UpdatePopulation()
    {
        Pet_Manager pet_Manager = Pet_Manager.Instance;
        TextPopulation.text = pet_Manager.Population.ToString();
        TextPopulationDogs.text = pet_Manager.PopulationDogs.ToString();
        TextPopulationCats.text = pet_Manager.PopulationCats.ToString();
        TextPopulationDeers.text = pet_Manager.PopulationDeers.ToString();
        TextPopulationWolves.text = pet_Manager.PopulationWolves.ToString();
        TextPopulationTigers.text = pet_Manager.PopulationTigers.ToString();
        TextPopulationBears.text = pet_Manager.PopulationBears.ToString();
        TextPopulationZombies.text = pet_Manager.PopulationZombies.ToString();
        TextPopulationPokemons.text = pet_Manager.PopulationPokemons.ToString();
    }

    void UpdateHappinessBar()
    {
        HappinessBar.fillAmount = Pet_Manager.Instance.GetAverageHappiness() / 100f;
    }

    void UpdateSentienceBar()
    {
        SentienceBar.fillAmount = Pet_Manager.Instance.GetAverageSentience() / 100f;
    }

    void UpdateBirthRate()
    {
        TextBirthRate.text = Pet_Manager.Instance.BirthRate.ToString();
    }

    void UpdateWeatherUI()
    {
        if (Weather_Manager.Instance.Raining)
        {
            TextWeather.text = "Raining";
        }
        if (Weather_Manager.Instance.Snowing)
        {
            TextWeather.text = "Snowing";
        }
        else
        {
            TextWeather.text = "Sunny";
        }
    }
}   