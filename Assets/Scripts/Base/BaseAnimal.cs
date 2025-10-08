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

    public BaseBehavior Behavior { get; protected set; }
    public BasePhysics Physics { get; protected set; }
    public BaseAnimator Animator { get; protected set; }

    public Rigidbody2D Rb2D { get; protected set; }
    public NavMeshAgent Agent { get; protected set; }
    public BaseAnimal BreedingPartner { get; set; }
    public const float BreedingChance = 1f; //100%

    public bool AllowBumping { get; set; }
    public GameObject Baby;

    protected virtual void Awake()
    {
        AllowBumping = true;
        Behavior = GetComponent<BaseBehavior>();
        Agent = GetComponent<NavMeshAgent>();
        Rb2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        // Set IDLE state as the default state
        Behavior.SetState(new State_IDLE(this));
    }

    public virtual void GiveBirth(BaseAnimal breedingPartner)
    {
        if (Sex == Sex.Female)
        {
            Vector2 midPoint = (transform.position + breedingPartner.transform.position) / 2f;
            Instantiate(Baby, midPoint, Quaternion.identity);
        }
    }
}


