using UnityEngine;

public class State_WolfBumping : StateTypeWolf
{
    float counter = 0f;
    const float pushingForce = 2f;

    public State_WolfBumping(Wolf _wolf) : base(_wolf) { }

    public override void OnStateEnter()
    {
        wolf.Animator.IsBeingBumped = true;
        if (wolf.CurrentPrey != null)
        {
            wolf.Physics.PushAnimal(wolf, wolf.CurrentPrey, pushingForce);
        }
        else
        {
            Debug.Log("Current prey is null!");
        }
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
        if (counter >= BaseAnimal.BumpingCooldown)
        {
            wolf.Behavior.SetState(new State_HuntPrey(wolf));
            counter = 0;
        }
    }

    public override void OnStateExit()
    {
        wolf.Animator.IsBeingBumped = false;
        wolf.Physics.IsTouchingAgent = false;
        wolf.Physics.CleanUpAfterPush(wolf);
    }
}
