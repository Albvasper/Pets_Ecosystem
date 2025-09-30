using UnityEngine;

public class State_WolfBumping : State
{
    float counter = 0f;
    const float pushingForce = 2f;
    GameObject otherAnimal;

    public State_WolfBumping(Wolf _wolf, GameObject other) : base(_wolf)
    {
        this.otherAnimal = other;
    }

    public override void OnStateEnter()
    {
        wolf.WolfAnimator.isBeingBumped = true;
        wolf.WolfPhysics.PushAnimal(otherAnimal, pushingForce);
    }

    // Behavior
    /*  Dizzy:
        - Make a dizzy animation
        - Wait n seconds
        - Go back to IDLE
    */
    public override void Tick()
    {
        counter += Time.deltaTime;
        if (counter >= Animal.BumpingCooldown)
        {
            wolf.WolfBehavior.SetState(new State_HuntPrey(wolf, otherAnimal));
            counter = 0;
        }
    }

    public override void OnStateExit()
    {
        wolf.WolfAnimator.isBeingBumped = false;
        wolf.WolfPhysics.IsTouchingAgent = false;
        wolf.WolfPhysics.CleanUpAfterPush();
    }
}
