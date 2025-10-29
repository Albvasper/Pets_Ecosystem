using UnityEngine;

/// <summary>
/// Attacking state for hostile pets.
/// Transitions to IDLE state.
/// </summary>
public class State_Attack : StateTypeHostilePet
{
    const float CoolDown = 1f;      // Attacking cooldown duration
    float counter = 0f;             // Counter for attacking state
    BaseAnimal prey;

    public State_Attack(HostilePet _pet, BaseAnimal _prey) : base(_pet)
    {
        prey = _prey;
    }

    public override void OnStateEnter()
    {
        // Check prey
        if (prey == null || prey.isDead)
        {
            pet.Behavior.SetState(new State_IDLE(pet));
            return;
        }
        pet.Behavior.StopWalking();
        pet.IsAttacking = true;
        pet.IsHunting = false;
        pet.Heal();
        prey.TakeDamage();
    }

    public override void Tick()
    {
        // Check prey
        if (prey == null || prey.isDead)
        {
            pet.Behavior.SetState(new State_IDLE(pet));
            return;
        }
        counter += Time.deltaTime;
        if (counter >= CoolDown)
        {
            pet.Behavior.AddHappiness(20);
            pet.Behavior.SetState(new State_IDLE(pet));
            counter = 0;
        }
    }

    public override void OnStateExit()
    {
        pet.IsAttacking = false;
        pet.CurrentPrey = null;
        pet.Behavior.KeepWalking();
    }
}
