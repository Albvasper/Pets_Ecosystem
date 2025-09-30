using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [SerializeField] AnimalData animalData;
    public AnimalBehavior AnimalBehavior { get; protected set; }
    public AnimalPhysics AnimalPhysics { get; protected set; }
    public AnimalAnimator AnimalAnimator { get; protected set; }
    public Rigidbody2D Rb2D { get; protected set; }
    public NavMeshAgent Agent { get; protected set; }
    // bumping state vars
    public const float BumpingCooldown = 3f;
    protected float counter = BumpingCooldown;

    protected virtual void Awake()
    {
        AnimalBehavior = GetComponent<AnimalBehavior>();
        AnimalPhysics = GetComponent<AnimalPhysics>();
        AnimalAnimator = GetComponent<AnimalAnimator>();
        Agent = GetComponent<NavMeshAgent>();
        Rb2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        // Set IDLE state as the default state
        AnimalBehavior.SetState(new State_IDLE(this));
    }

    protected virtual void Update()
    {
        // If an animal is touching another agent => cooldown from changing states
        counter += Time.deltaTime;
        if (AnimalPhysics.IsTouchingAgent)
        {
            if (counter >= BumpingCooldown)
            {
                AnimalBehavior.SetState(new State_Bumping(this, AnimalPhysics.BumpingAnimal));
                counter = 0;
            }
        }
    }
}