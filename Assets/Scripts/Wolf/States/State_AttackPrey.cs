using UnityEngine;

public class State_AttackPrey : State
{
    float counter = 0f;
    const float coolDown = 2f;
    
    public State_AttackPrey(Wolf _wolf, Animal _prey) : base(_wolf, _prey) { }

    public override void OnStateEnter()
    {
        wolf.AnimalBehavior.StopWalking();
    }

    public override void Tick()
    {
        wolf.WolfAnimator.Attack();
        counter += Time.deltaTime;
        if (counter >= coolDown)
        { 
            wolf.WolfBehavior.SetState(new State_IDLE(wolf));
            counter = 0;
        }
    }

    public override void OnStateExit()
    {
        wolf.WolfAnimator.IDLE();
        wolf.Hunting = false;
        wolf.WolfPhysics.BumpingAnimal = null;
        wolf.WolfBehavior.KeepWalking();
    }
}
