using UnityEngine;

public class State_AttackPrey : StateTypeWolf
{
    Animal prey;
    float counter = 0f;
    const float coolDown = 1f;

    public State_AttackPrey(Wolf _wolf, Animal _prey) : base(_wolf)
    {
        prey = _prey;
    }

    public override void OnStateEnter()
    {   
        wolf.Behavior.StopWalking();
        wolf.IsAttacking = true;
        wolf.IsHunting = false;
    }

    public override void Tick()
    {
        counter += Time.deltaTime;
        if (counter >= coolDown)
        { 
            wolf.Behavior.AddHappiness(20);
            wolf.Behavior.SetState(new State_IDLE(wolf));
            counter = 0;
        }
    }

    public override void OnStateExit()
    {
        wolf.IsAttacking = false;
        wolf.CurrentPrey = null;
        wolf.Behavior.KeepWalking();
    }
}
