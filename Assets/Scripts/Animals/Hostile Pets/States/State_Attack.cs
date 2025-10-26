using UnityEngine;

public class State_Attack : StateTypeHostilePet
{
    BaseAnimal prey;
    float counter = 0f;
    const float coolDown = 1f;

    public State_Attack(HostilePet _pet, BaseAnimal _prey) : base(_pet)
    {
        prey = _prey;
    }

    public override void OnStateEnter()
    {   
        pet.Behavior.StopWalking();
        pet.IsAttacking = true;
        pet.IsHunting = false;
        pet.Heal();
        prey.TakeDamage();
    }

    public override void Tick()
    {
        counter += Time.deltaTime;
        if (counter >= coolDown)
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
