using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BaseBehavior))]
[RequireComponent(typeof(WolfPhysics))]
[RequireComponent(typeof(WolfAnimator))]
public class Wolf : BaseAnimal
{
    public WolfPhysics Physics { get; private set; }
    public WolfAnimator Animator { get; private set; }
    public bool IsHunting { get; set; }
    public bool IsAttacking { get; set; }
    public Animal CurrentPrey { get; set;}

    protected override void Awake()
    {
        base.Awake();
        Physics = GetComponent<WolfPhysics>();
        Animator = GetComponent<WolfAnimator>();
        IsHunting = false;
        IsAttacking = false;
    }

    protected virtual void Start()
    {
        // Set IDLE state as the default state
        Behavior.SetState(new State_IDLE(this));
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // If an animal is touching another agent => cooldown from changing states
        counter += Time.deltaTime;
        if (!IsHunting && Physics.IsTouchingAgent)
        {
            if (counter >= BumpingCooldown)
            {
                Debug.Log("just bumping");
                Behavior.SetState(new State_WolfBumping(this));
                counter = 0;
            }
        }
        else if (IsHunting && Physics.IsTouchingAgent)
        {
            if (counter >= BumpingCooldown)
            {
                Debug.Log("IS GOING TO ATTACK!");
                Behavior.SetState(new State_AttackPrey(this));
                counter = 0;
            }
        }
    }
}
