using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{

    [SerializeField] AnimalData animalData;
    AnimalBehavior animalBehavior;
    AnimalPhysics animalPhysics;
    Rigidbody2D rb2D;
    NavMeshAgent agent;
    // bumping state vars
    const float cooldown = 5f;
    float counter = cooldown;

    private void Awake()
    {
        animalBehavior = GetComponent<AnimalBehavior>();
        animalPhysics = GetComponent<AnimalPhysics>();
        agent = GetComponent<NavMeshAgent>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Set IDLE state as the default state
        animalBehavior.SetState(new State_IDLE(this));

    }

    private void Update()
    {
        // If an animal is touching another agent => cooldown from changing states
        counter += Time.deltaTime;
        if (animalPhysics.IsTouchingAgent())
        {
            if (counter >= cooldown)
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

    public AnimalBehavior GetAnimalBehavior()
    {
        return animalBehavior;
    }

    public AnimalPhysics GetAnimalPhysics()
    {
        return animalPhysics;
    }

    public float GetBumpingCooldown()
    {
        return cooldown;
    }
}