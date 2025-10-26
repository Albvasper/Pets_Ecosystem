using UnityEngine;
using UnityEngine.AI;
using TMPro;
using Firebase.Extensions;

public enum TypeOfPet { Cat, Dog, Deer, Wolf, Bear, Tiger }
public enum Friendliness { Passive, Hostile }
public enum Sex { Male, Female }

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseAnimal : MonoBehaviour
{
    public string petName;
    public string petID;

    public TypeOfPet TypeOfPet;
    public Friendliness Friendliness;
    public Sex Sex;

    public BaseBehavior Behavior { get; protected set; }
    public BasePhysics Physics { get; protected set; }
    public BaseAnimator Animator { get; protected set; }
    
    public Rigidbody2D Rb2D { get; protected set; }
    public NavMeshAgent Agent { get; protected set; }
    public TextMeshProUGUI nameTag;
   
    //protected AnimalData animalData;
    [SerializeField] protected GameObject Baby;
    public BaseAnimal PackLeader { get; set; }
    public bool CanHaveKids;
    public BaseAnimal BreedingPartner { get; set; }

    public float Happiness { get; set; } // 0-100
    public float Sentience { get; set; } // 0-100

    public bool IsZombie { get; set;  } // 0-100

    public const float BreedingChance = 1f; //100%
    const float breedingCooldown = 30f;
    float counter;

    protected virtual void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Rb2D = GetComponent<Rigidbody2D>();
        Happiness = 50f;
        Sentience = 10f;
        CanHaveKids = false;
        IsZombie = false;
    }
    
    protected virtual void Start()
    {
        // Set the animal sex randomly
        Sex = Random.value < 0.5f ? Sex.Male : Sex.Female;
        // Add this to the animals list
        Pet_Manager.Instance.Pets.Add(this);
        Pet_Manager.Instance.RegisterBirth();
        // Set IDLE state as the default state
        Behavior.SetState(new State_IDLE(this));
        // Add animal to population count
        switch (TypeOfPet)
        {
            case TypeOfPet.Dog:
                Pet_Manager.Instance.AddToDogPopulation();
            break;
            
            case TypeOfPet.Cat:
                Pet_Manager.Instance.AddToCatPopulation();
            break;
            
            case TypeOfPet.Deer:
                Pet_Manager.Instance.AddToDeerPopulation();
            break;
            
            case TypeOfPet.Wolf:
                Pet_Manager.Instance.AddToWolfPopulation();
            break;

            case TypeOfPet.Tiger:
                Pet_Manager.Instance.AddToTigerPopulation();
            break;

            case TypeOfPet.Bear:
                Pet_Manager.Instance.AddToBearPopulation();
            break;
        }
    }

    protected virtual void Update()
    {
        if (CanHaveKids == false)
        {
            counter += Time.deltaTime;
            if (counter >= breedingCooldown)
            {
                CanHaveKids = true;
                counter = 0;
            }
        }
    }

    public void SetPetID(string id)
    {
        petID = id;
    }

    public void SetPetName(string _name)
    {
        name = _name;
        nameTag.text = _name;
    }
    
    //TODO: MOVE THIS TO THE BASE BEHAVIOR CLASS
    public virtual void GiveBirth(BaseAnimal breedingPartner)
    {
        PackLeader = breedingPartner;
        Vector2 midPoint = (transform.position + breedingPartner.transform.position) / 2f;
        BaseAnimal b = Instantiate(Baby, midPoint, Quaternion.identity).GetComponent<BaseAnimal>();
        b.PackLeader = breedingPartner;
        b.Behavior.AddSentience(11);
    }

    void OnDestroy()
    {

        Pet_Manager.Instance.Pets.Remove(this);

        switch (TypeOfPet)
        {
            case TypeOfPet.Dog:
                Pet_Manager.Instance.RemoveFromDogPopulation();
                break;

            case TypeOfPet.Cat:
                Pet_Manager.Instance.RemoveFromCatPopulation();
                break;

            case TypeOfPet.Deer:
                Pet_Manager.Instance.RemoveFromDeerPopulation();
                break;

            case TypeOfPet.Wolf:
                Pet_Manager.Instance.RemoveFromWolfPopulation();
                break;

            case TypeOfPet.Tiger:
                Pet_Manager.Instance.RemoveFromTigerPopulation();
                break;

            case TypeOfPet.Bear:
                Pet_Manager.Instance.RemoveFromBearPopulation();
                break;
        }
        
        if (FirebaseInit.db == null || string.IsNullOrEmpty(petID)) return;

        FirebaseInit.db.Child("ecosystem").Child("pets").Child(petID)
            .RemoveValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
                Debug.Log($"Removed pet {petName} ({petID}) from Firebase");
        });
    }
}


