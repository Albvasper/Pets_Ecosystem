using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum TypeOfPet { Cat, Dog, Deer, Wolf }
public enum Sex { Male, Female }

[RequireComponent(typeof(BaseBehavior))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseAnimal : MonoBehaviour
{
    public TypeOfPet TypeOfPet;
    public Sex Sex;
    protected AnimalData animalData;
    [SerializeField] protected GameObject Baby;
    public BaseAnimal PackLeader { get; set; }
    public bool CanHaveKids;

    public BaseBehavior Behavior { get; protected set; }
    public BasePhysics Physics { get; protected set; }
    public BaseAnimator Animator { get; protected set; }

    public Rigidbody2D Rb2D { get; protected set; }
    public NavMeshAgent Agent { get; protected set; }
    public BaseAnimal BreedingPartner { get; set; }
    public const float BreedingChance = 1f; //100%

    const float breedingCooldown = 30f;
    float counter;

    protected virtual void Awake()
    {
        CanHaveKids = false;
        Behavior = GetComponent<BaseBehavior>();
        Agent = GetComponent<NavMeshAgent>();
        Rb2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        // Set IDLE state as the default state
        Behavior.SetState(new State_IDLE(this));
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

    public virtual void GiveBirth(BaseAnimal breedingPartner)
    {
        PackLeader = breedingPartner;
        Vector2 midPoint = (transform.position + breedingPartner.transform.position) / 2f;
        BaseAnimal b = Instantiate(Baby, midPoint, Quaternion.identity).GetComponent<BaseAnimal>();
        b.PackLeader = breedingPartner;
    }
}


