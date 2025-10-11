using UnityEngine;

public class State_WolfBumping : StateTypeWolf
{
    float counter = 0f;
    const float PushingForce = 2f;
    const float BumpingCooldown = 1.5f;
    BaseAnimal otherAnimal;

    public State_WolfBumping(Wolf _wolf, BaseAnimal _otherAnimal) : base(_wolf)
    {
        otherAnimal = _otherAnimal;
    }

    public override void OnStateEnter()
    {
        wolf.Animator.IsBeingBumped = true;
        wolf.Physics.PushAnimal(wolf, otherAnimal, PushingForce);
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
        // When the bumping cooldown has ended
        if (counter >= BumpingCooldown)
        {
            wolf.Behavior.SubstractHappiness(1);
            // If the animal has a breeding partner: mate
            if (wolf.BreedingPartner != null && wolf.CanHaveKids && otherAnimal.CanHaveKids)
            {
                wolf.Behavior.SetState(new State_Breeding(wolf, otherAnimal));
            }
            // If wolf has a current prey
            else if (wolf.CurrentPrey != null)
            {
                wolf.Behavior.SetState(new State_HuntPrey(wolf, wolf.CurrentPrey));
            }
            // If breeding is not possible do IDLE
            else
            {
                wolf.Behavior.SetState(new State_IDLE(wolf));
            }
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
