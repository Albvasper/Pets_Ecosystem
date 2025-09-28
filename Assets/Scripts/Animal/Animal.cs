using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{

    [SerializeField] AnimalData animalData;
    public AnimalBehavior AnimalBehavior { get; private set; }
    public AnimalPhysics AnimalPhysics { get; private set; }
    public AnimalAnimator AnimalAnimator { get; private set; }
    public Rigidbody2D Rb2D { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    // bumping state vars
    public const float Cooldown = 5f;
    float counter = Cooldown;

    private void Awake()
    {
        AnimalBehavior = GetComponent<AnimalBehavior>();
        AnimalPhysics = GetComponent<AnimalPhysics>();
        AnimalAnimator = GetComponent<AnimalAnimator>();
        Agent = GetComponent<NavMeshAgent>();
        Rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Set IDLE state as the default state
        AnimalBehavior.SetState(new State_IDLE(this));

    }

    private void Update()
    {
        // If an animal is touching another agent => cooldown from changing states
        counter += Time.deltaTime;
        if (AnimalPhysics.IsTouchingAgent)
        {
            if (counter >= Cooldown)
            {
                AnimalBehavior.SetState(new State_Bumping(this, AnimalPhysics.BumpingAnimal));
                counter = 0;
            }
        }
    }
}