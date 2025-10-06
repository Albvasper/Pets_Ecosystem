using UnityEngine;

[RequireComponent(typeof(BaseAnimal))]
public class BaseBehavior : MonoBehaviour
{
    protected BaseAnimal animal;
    protected State CurrentState { get; set; }

    protected virtual void Awake()
    {
        animal = GetComponent<BaseAnimal>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CurrentState.Tick();
    }

    // Set a new state for the animal
    public virtual void SetState(State state)
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
    public virtual void Walk(Vector2 target)
    {
        animal.Agent.SetDestination(
            Vector2.Lerp(
                animal.transform.position,
                target,
                1f
            ));
    }

    public virtual void StopWalking()
    {
        animal.Agent.isStopped = true;
        animal.Agent.enabled = false;
    }

    public virtual void KeepWalking()
    {
        animal.Agent.enabled = true;
        animal.Agent.isStopped = false;
    }
}
