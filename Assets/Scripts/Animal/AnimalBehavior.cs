using System;
using UnityEngine;
using UnityEngine.AI;

public class AnimalBehavior : MonoBehaviour
{
    Animal animal;
    State currentState;

    void Awake()
    {
        animal = GetComponent<Animal>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentState.Tick();
    }

    // Set a new state for the animal
    public void SetState(State state)
    {
        if (currentState != null)
        {
            // Get out of the current state
            currentState.OnStateExit();
        }
        // Replace current state with new state
        currentState = state;
        if (currentState != null)
        {
            // Initialize new state
            currentState.OnStateEnter();
        }
    }

    // Move the agent to the target
    public void Walk(Vector2 target)
    {
        animal.GetNavMeshAgent().SetDestination(
            Vector2.Lerp(
                animal.transform.position,
                target,
                1f
            ));
    }

    public void StopWalking()
    {
        animal.GetNavMeshAgent().isStopped = true;
        animal.GetNavMeshAgent().enabled = false;
    }

    public void KeepWalking()
    {
        animal.GetNavMeshAgent().enabled = true;
        animal.GetNavMeshAgent().isStopped = false;

    }
}
