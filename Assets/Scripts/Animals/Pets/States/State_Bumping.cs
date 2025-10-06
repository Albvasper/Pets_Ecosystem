using UnityEngine;

public class State_Bumping : StateTypePets
{
    float counter = 0f;
    const float pushingForce = 2f;
    GameObject otherAnimal;

    public State_Bumping(Animal _animal, GameObject _otherAnimal) : base(_animal)
    {
        otherAnimal =  _otherAnimal;
    }

    public override void OnStateEnter()
    {
        animal.Animator.IsBeingBumped = true;
        animal.Physics.PushAnimal(animal, otherAnimal, pushingForce);
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
            animal.Behavior.SetState(new State_IDLE(animal));
            counter = 0;
        }
    }

    public override void OnStateExit()
    {
        animal.Animator.IsBeingBumped = false;
        animal.Physics.IsTouchingAgent = false;
        animal.Physics.CleanUpAfterPush(animal);
    }
}
