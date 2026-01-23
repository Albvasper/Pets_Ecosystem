using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClientEcosystemUiManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI birthRateText;
    [SerializeField] TextMeshProUGUI petCountText;
    [SerializeField] TextMeshProUGUI dogCountText;
    [SerializeField] TextMeshProUGUI catCountText;
    [SerializeField] TextMeshProUGUI deerCountText;
    [SerializeField] TextMeshProUGUI bearCountText;
    [SerializeField] TextMeshProUGUI tigerCountText;
    [SerializeField] TextMeshProUGUI wolfCountText;
    [SerializeField] TextMeshProUGUI weatherText;
    [SerializeField] Image happinessBar;
    [SerializeField] Image sentienceBar;

    private static int petCount = 0;
    private static int dogsCount = 0;
    private static int catsCount = 0;
    private static int deersCount = 0;
    private static int bearsCount = 0;
    private static int tigersCount = 0;
    private static int wolfsCount = 0;
    private static string currentWeather;
    private static float birthRate;
    private static float currentpopulationHappiness;
    private static float currentpopulationSentience;
    
    void Update()
    {
        petCountText.text = petCount.ToString();
        dogCountText.text = dogsCount.ToString();
        catCountText.text = catsCount.ToString();
        deerCountText.text = deersCount.ToString();
        bearCountText.text = bearsCount.ToString();
        tigerCountText.text = tigersCount.ToString();
        wolfCountText.text = wolfsCount.ToString();
        weatherText.text = currentWeather;
        birthRateText.text = birthRate.ToString();
        UpdateHappinessBar();
        UpdateSentienceBar();
    }

    public static void AddToPetCount()
    {
        petCount++;
    }

    public static void RemoveFromPetCount()
    {
        petCount--;
    }

    public static void AddToDogCount()
    {
        dogsCount++;
    }

    public static void RemoveFromDogCount()
    {
        dogsCount--;
    }

    public static void AddToCatCount()
    {
        catsCount++;
    }

    public static void RemoveFromCatCount()
    {
        catsCount--;
    }

    public static void AddToDeerCount()
    {
        deersCount++;
    }

    public static void RemoveFromDeerCount()
    {
        deersCount--;
    }

    public static void AddToBearCount()
    {
        bearsCount++;
    }

    public static void RemoveFromBearCount()
    {
        bearsCount--;
    }

    public static void AddToTigerCount()
    {
        tigersCount++;
    }

    public static void RemoveFromTigerCount()
    {
        tigersCount--;
    }

    public static void AddToWolfCount()
    {
        wolfsCount++;
    }

    public static void RemoveFromWolfCount()
    {
        wolfsCount--;
    }

    public static void ChangeWeatherText(string newWeather)
    {
        currentWeather = newWeather;
    }

    public static void UpdateBirthRate(float newBirthRate)
    {
        birthRate = newBirthRate;
    }

    public static void UpdateHappiness(float newHappiness)
    {
        currentpopulationHappiness = newHappiness;
    }

    public static void UpdateSentience(float newSentience)
    {
        currentpopulationSentience = newSentience;
    }

    void UpdateHappinessBar()
    {
        happinessBar.fillAmount =  currentpopulationHappiness/ 100f;
    }

    void UpdateSentienceBar()
    {
        sentienceBar.fillAmount = currentpopulationSentience / 100f;
    }


    public void BackToMenu()
    {
        SceneManager.LoadScene ("ViewerScreen");
    }
}
