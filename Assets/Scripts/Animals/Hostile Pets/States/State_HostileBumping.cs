using UnityEngine;

/// <summary>
/// Bumping state for hostile pets. When a hostile pet and a peaceful pet
/// collide, they will be stunned for a couple of seconds, then the hostile
/// pet will attack the peaceful pet.
/// Transitions to breeding, hunt and IDLE state.
/// </summary>
public class State_HostileBumping : StateTypeHostilePet
{
    float counter = 0f;
    const float PushingForce = 2f;
    const float BumpingCooldown = 1.5f;
    BaseAnimal otherAnimal;

    public State_HostileBumping(HostilePet _pet, BaseAnimal _otherAnimal) : base(_pet)
    {
        otherAnimal = _otherAnimal;
    }

    public override void OnStateEnter()
    {
        // Check other animal
        if (otherAnimal == null || otherAnimal.isDead)
        {
            pet.Behavior.SetState(new State_IDLE(pet));
            return;
        }
        pet.Animator.IsBeingBumped = true;
        pet.Physics.PushAnimal(pet, otherAnimal, PushingForce);
    }

    public override void Tick()
    {
        // Check other animal
        if (otherAnimal == null || otherAnimal.isDead)
        {
            pet.Behavior.SetState(new State_IDLE(pet));
            return;
        }
        counter += Time.deltaTime;
        // When the bumping cooldown has ended
        if (counter >= BumpingCooldown)
        {
            pet.Behavior.SubstractHappiness(1);
            // If the animal has a breeding partner -> mate
            if (pet.BreedingPartner != null && !pet.BreedingPartner.isDead
                && pet.CanHaveKids && otherAnimal.CanHaveKids)
            {
                pet.Behavior.SetState(new State_Breeding(pet, otherAnimal));
            }
            // If pet has a current prey
            else if (pet.CurrentPrey != null && !pet.CurrentPrey.isDead)
            {
                pet.Behavior.SetState(new State_Hunt(pet, pet.CurrentPrey));
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
