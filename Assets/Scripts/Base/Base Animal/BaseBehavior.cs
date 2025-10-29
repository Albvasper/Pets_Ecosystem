using UnityEngine;

/// <summary>
/// Handles the state machine and general behavior for all pets. 
/// Changes states and walking behavior.
/// </summary>
[RequireComponent(typeof(BaseAnimal))]
public class BaseBehavior : MonoBehaviour
{
    protected BaseAnimal animal;
    protected State CurrentState { get; set; }

    protected virtual void Awake()
    {
        animal = GetComponent<BaseAnimal>();
    }

    protected virtual void Update()
    {
        if (!animal.isDead)
        {
            CurrentState.Tick();
            UpdateHappiness();
            UpdateSentience();
        }
    }

    /// <summary>
    /// Set a new state for the animal.
    /// </summary>
    /// <param name="state">New state</param>
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

    /// <summary>
    /// Move the pet using pathfinding to a destination.
    /// </summary>
    /// <param name="target">Destination</param>
    public virtual void Walk(Vector2 target)
    {
        animal.Agent.SetDestination(
            Vector2.Lerp(
                animal.transform.position,
                target,
                1f
            ));
    }

    /// <summary>
    /// Disable nav mesh agent
    /// </summary>
    public virtual void StopWalking()
    {
        animal.Agent.isStopped = true;
        animal.Agent.enabled = false;
    }

    /// <summary>
    /// Enables nav mesh agent
    /// </summary>
    public virtual void KeepWalking()
    {
        animal.Agent.enabled = true;
        animal.Agent.isStopped = false;
    }
    
    public void AddHappiness(float amount)
    {
        Pet_Manager.Instance.NotifyHappinessChanged();
        animal.Happiness = Mathf.Min(animal.Happiness + amount, 100f);
    }

    public void SubstractHappiness(float amount)
    {
        Pet_Manager.Instance.NotifyHappinessChanged();
        animal.Happiness = Mathf.Max(animal.Happiness - amount, 0f);
    }

    public virtual void UpdateHappiness()
    {
        Pet_Manager.Instance.NotifyHappinessChanged();
        animal.Happiness = Mathf.MoveTowards(animal.Happiness, 50f, Time.deltaTime * 0.5f);
        if (animal.PackLeader != null)
            animal.Happiness = Mathf.Min(animal.Happiness + Time.deltaTime * 1f, 80f);
    }

    public void AddSentience(float amount)
    {
        Pet_Manager.Instance.NotifySentienceChanged();
        animal.Sentience = Mathf.Min(animal.Sentience + amount, 100f);
    }

    public virtual void UpdateSentience()
    {
        Pet_Manager.Instance.NotifySentienceChanged();
        if (animal.PackLeader != null)
            animal.Sentience = Mathf.Min(animal.Sentience + Time.deltaTime * 0.2f, 20f);
    }
}
