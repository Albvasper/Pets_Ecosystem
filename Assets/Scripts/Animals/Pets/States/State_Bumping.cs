using UnityEngine;

/// <summary>
/// Bumping state for animals. When two pets collide they will be stunned for
/// a couple of seconds.
/// Transitions to breeding state and IDLE state.
/// </summary>
public class State_Bumping : StateTypePets
{
    const float PushingForce = 2f;
    const float BumpingCooldown = 1.5f;
    float counter = 0f;                     // Bumping cooldown counter.
    BaseAnimal otherAnimal;

    public State_Bumping(Pet _pet, BaseAnimal _otherAnimal) : base(_pet)
    {
        otherAnimal = _otherAnimal;
    }

    public override void OnStateEnter()
    {
        pet.Animator.IsBeingBumped = true;
        if (otherAnimal != null && !otherAnimal.isDead)
            pet.Physics.PushAnimal(pet, otherAnimal, PushingForce);
    }

    public override void Tick()
    {
        counter += Time.deltaTime;
        // When the bumping cooldown has ended
        if (counter >= BumpingCooldown)
        {
            pet.Behavior.SubstractHappiness(1);
            // TODO: IF ANIMAL IS ALSO TOUCHGING BREEDING PARTNER OR JUST COLLIDED WITH IT THEN MATES
            // If the animal has a breeding partner: mate
            if (pet.BreedingPartner != null && !pet.BreedingPartner.isDead
                && pet.CanHaveKids && otherAnimal.CanHaveKids)
            {
                // Breeding process
                pet.Behavior.SetState(new State_Breeding(pet, otherAnimal));
            }
            // If breeding is not possible do IDLE
            else
            {
                pet.Behavior.SetState(new State_IDLE(pet));
            }
            counter = 0;
        }
    }

    public override void OnStateExit()
    {
        pet.Animator.IsBeingBumped = false;
        pet.Physics.IsTouchingAgent = false;
        pet.Physics.CleanUpAfterPush(pet);
    }
}
