using UnityEngine;

public class State_HuntPrey : StateTypeWolf
{

    public State_HuntPrey(Wolf _wolf) : base(_wolf) { }

    public override void OnStateEnter()
    {
        wolf.IsHunting = true;
    }

    public override void Tick()
    {
        wolf.Behavior.Walk(wolf.CurrentPrey.gameObject.transform.position);
    }
}
