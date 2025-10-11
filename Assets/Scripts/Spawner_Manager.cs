using UnityEngine;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;

public class Spawner_Manager : MonoBehaviour
{

    public static Spawner_Manager Instance { get; private set; }

    [SerializeField] GameObject SpawnPoint;
    [SerializeField] GameObject DogPet;
    [SerializeField] GameObject CatPet;
    [SerializeField] GameObject DeerPet;
    [SerializeField] GameObject WolfPet;

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
        Web3.OnBalanceChange += OnBalanceChanged;
    }

    void OnDisable()
    {
        Web3.OnBalanceChange -= OnBalanceChanged;
    }

    public void OnBalanceChanged(double solBalance)
    {
        int animalsToSpawn = (int)solBalance; 
        SpawnPet(animalsToSpawn);
    }

    void SpawnPet(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnRandomPet();
        }
    }
    
    void SpawnRandomPet()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                // Dog
                Instantiate(DogPet, SpawnPoint.transform.position, Quaternion.identity);
                break;
            case 1:
                // Cat
                Instantiate(CatPet, SpawnPoint.transform.position, Quaternion.identity);
                break;
            case 2:
                // Deer
                Instantiate(DeerPet, SpawnPoint.transform.position, Quaternion.identity);
                break;
            case 3:
                // Wolf
                Instantiate(WolfPet, SpawnPoint.transform.position, Quaternion.identity);
                break;
        }
    }
}
