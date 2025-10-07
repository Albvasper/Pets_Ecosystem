using UnityEngine;

public class State_Bumping : StateTypePets
{
    float counter = 0f;
    const float pushingForce = 2f;
    BaseAnimal otherAnimal;

    public State_Bumping(Animal _animal, BaseAnimal _otherAnimal) : base(_animal)
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
        // When the bumping cooldown has ended
        if (counter >= BaseAnimal.BumpingCooldown)
        {
            // If the animal has a breeding partner take a chance
            if (animal.BreedingPartner != null)
            {
                // Take a chance of breeding
                if (Random.value < BaseAnimal.BreedingChance)
                {
                    // Breeding process
                    animal.Behavior.SetState(new State_Breeding(animal, otherAnimal));
                    counter = 0;
                }
                else
                {
                    // If breeding is not possible do IDLE
                    animal.Behavior.SetState(new State_IDLE(animal));
                    counter = 0;
                }
            }
            else
            {
                // If breeding is not possible do IDLE
                animal.Behavior.SetState(new State_IDLE(animal));
                counter = 0;
            }
        }
    }

    public override void OnStateExit()
    {
        animal.Animator.IsBeingBumped = false;
        animal.Physics.IsTouchingAgent = false;
        animal.Physics.CleanUpAfterPush(animal);
    }
}
