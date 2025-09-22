using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{

    [SerializeField] AnimalData animalData;
    public AnimalBehavior animalBehavior;
    public AnimalPhysics animalPhysics;
    Rigidbody2D rb2D;
    NavMeshAgent agent;

    // bumping state vars
    const float limit = 4f;
    float counter = limit;

    private void Awake()
    {
        animalBehavior = GetComponent<AnimalBehavior>();
        agent = GetComponent<NavMeshAgent>();
        animalPhysics = GetComponentInChildren<AnimalPhysics>();
        rb2D = GetComponentInChildren<Rigidbody2D>();
        // Set IDLE state as the default state
        animalBehavior.SetState(new State_IDLE(this));
    }

    private void Start()
    {

    }

    private void Update()
    {
        // If an animal is touching another agent => cooldown from changing states
        counter += Time.deltaTime;
        if (animalPhysics.IsTouchingAgent())
        {
            if (counter >= limit)
            {
                animalBehavior.SetState(new State_Bumping(this, animalPhysics.GetBumpingAnimal()));
                counter = 0;
            }
        }
    }

    public Rigidbody2D GetRigidbody2D()
    {
        return rb2D;
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return agent;
    }

    public void SetNavMeshAgent(NavMeshAgent _agent)
    {
        agent = _agent;
    }
}
