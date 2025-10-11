using System.Xml.Serialization;
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
        UpdateHappiness();
        UpdateSentience();
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
