using UnityEngine;

public class State_Bumping : StateTypePets
{
    float counter = 0f;
    const float PushingForce = 2f;
    const float BumpingCooldown = 3f;
    BaseAnimal otherAnimal;

    public State_Bumping(Animal _animal, BaseAnimal _otherAnimal) : base(_animal)
    {
        otherAnimal = _otherAnimal;
    }

    public override void OnStateEnter()
    {
        animal.Animator.IsBeingBumped = true;
        animal.Physics.PushAnimal(animal, otherAnimal, PushingForce);
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
            // If the animal has a breeding partner: mate
            if (animal.BreedingPartner != null && animal.CanHaveKids && otherAnimal.CanHaveKids)
            {
                // Breeding process
                animal.Behavior.SetState(new State_Breeding(animal, otherAnimal));
            }
            // If breeding is not possible do IDLE
            else
            {
                animal.Behavior.SetState(new State_IDLE(animal));
            }
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
