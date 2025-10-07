using UnityEngine;
using UnityEngine.AI;

public enum TypeOfPet { Cat, Dog, Deer, Wolf }

[RequireComponent(typeof(BaseBehavior))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseAnimal : MonoBehaviour
{
    [SerializeField] public bool CanGiveBirth;
    public TypeOfPet TypeOfPet;
    protected AnimalData animalData;
    public BaseBehavior Behavior { get; protected set; }
    public Rigidbody2D Rb2D { get; protected set; }
    public NavMeshAgent Agent { get; protected set; }
    public BaseAnimal BreedingPartner { get; set; }
    public const float BumpingCooldown = 3f;
    public const float BreedingChance = 1f; //100%
    protected float counter = BumpingCooldown;
    public bool AllowBumping { get; set; }
    public GameObject Baby;

    protected virtual void Awake()
    {
        AllowBumping = true;
        Behavior = GetComponent<BaseBehavior>();
        Agent = GetComponent<NavMeshAgent>();
        Rb2D = GetComponent<Rigidbody2D>();
    }

    public virtual void GiveBirth(BaseAnimal breedingPartner)
    {
        if (CanGiveBirth) {
            Vector2 midPoint = (transform.position + breedingPartner.transform.position) / 2f;
            Instantiate(Baby, midPoint, Quaternion.identity);
        }
    }
}


