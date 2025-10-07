using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BaseBehavior))]
[RequireComponent(typeof(AnimalPhysics))]
[RequireComponent(typeof(AnimalAnimator))]
public class Animal : BaseAnimal
{
    public AnimalPhysics Physics { get; protected set; }
    public AnimalAnimator Animator  { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        Physics = GetComponent<AnimalPhysics>();
        Animator = GetComponent<AnimalAnimator>();
    }

    protected virtual void Start()
    {
        // Set IDLE state as the default state
        Behavior.SetState(new State_IDLE(this));
    }
    
    protected virtual void Update()
    {
        // If an animal is touching another agent => cooldown from changing states
        if (AllowBumping)
        {
            counter += Time.deltaTime;
        }
        else
        {
            counter = 0;
        }
        if (Physics.IsTouchingAgent)
        {
            if (counter >= BumpingCooldown)
            {
                Behavior.SetState(new State_Bumping(this, Physics.BumpingAnimal));
                counter = 0;
            }
        }
    }
}