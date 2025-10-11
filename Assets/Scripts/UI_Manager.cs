using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance { get; private set; }

    public int Happiness { get; set; }
    public int Sentience { get; set; }
    [SerializeField] private TextMeshProUGUI TextPopulation;
    [SerializeField] private TextMeshProUGUI TextPopulationDogs;
    [SerializeField] private TextMeshProUGUI TextPopulationCats;
    [SerializeField] private TextMeshProUGUI TextPopulationDeers;
    [SerializeField] private TextMeshProUGUI TextPopulationWolves;
    [SerializeField] private TextMeshProUGUI TextPopulationZombies;
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
    }

    void Start()
    {
        Player.Instance.OnPopulationChanged += UpdatePopulation;
    }

    void OnDisable()
    {
        Player.Instance.OnPopulationChanged -= UpdatePopulation;
    }

    void UpdatePopulation()
    {
        Debug.Log("Update UI!");
        Player player = Player.Instance;
        TextPopulation.text = player.Population.ToString();
        TextPopulationDogs.text = player.PopulationDogs.ToString();
        TextPopulationCats.text = player.PopulationCats.ToString();
        TextPopulationDeers.text = player.PopulationDeers.ToString();
        TextPopulationWolves.text = player.PopulationWolves.ToString();
        TextPopulationZombies.text = player.PopulationZombies.ToString();
    }
}   