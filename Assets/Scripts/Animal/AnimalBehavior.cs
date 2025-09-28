using System;
using UnityEngine;
using UnityEngine.AI;

public class AnimalBehavior : MonoBehaviour
{
    Animal animal;
    State CurrentState { get; set; }

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
        CurrentState.Tick();
    }

    // Set a new state for the animal
    public void SetState(State state)
    {
        if (CurrentState != null)
        {
            // Get out of the current state
            CurrentState.OnStateExit();
        }
        // Replace current state with new state
        CurrentState = state;
        if (CurrentState != null)
        {
            // Initialize new state
            CurrentState.OnStateEnter();
        }
    }

    // Move the agent to the target
    public void Walk(Vector2 target)
    {
        animal.Agent.SetDestination(
            Vector2.Lerp(
                animal.transform.position,
                target,
                1f
            ));
    }

    public void StopWalking()
    {
        animal.Agent.isStopped = true;
        animal.Agent.enabled = false;
    }

    public void KeepWalking()
    {
        animal.Agent.enabled = true;
        animal.Agent.isStopped = false;

    }
}
