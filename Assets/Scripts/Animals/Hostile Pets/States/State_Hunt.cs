using UnityEngine;

public class State_Hunt : StateTypeHostilePet
{
    BaseAnimal prey;

    public State_Hunt(HostilePet _pet, BaseAnimal _prey) : base(_pet)
    {
        prey = _prey;
    }

    public override void OnStateEnter()
    {
        pet.IsHunting = true;
    }

    public override void Tick()
    {
        pet.Behavior.Walk(pet.CurrentPrey.gameObject.transform.position);
        if (pet.Physics.IsTouchingAgent)
        {
            pet.Behavior.SetState(new State_Attack(pet, prey));
        }
    }
}
