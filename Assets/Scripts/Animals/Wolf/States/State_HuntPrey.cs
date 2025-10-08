using UnityEngine;

public class State_HuntPrey : StateTypeWolf
{
    Animal prey;

    public State_HuntPrey(Wolf _wolf, Animal _prey) : base(_wolf) { }

    public override void OnStateEnter()
    {
        wolf.IsHunting = true;
    }

    public override void Tick()
    {
        wolf.Behavior.Walk(wolf.CurrentPrey.gameObject.transform.position);
        if (wolf.Physics.IsTouchingAgent)
        {
            wolf.Behavior.SetState(new State_AttackPrey(wolf, prey));
        }
    }
}
