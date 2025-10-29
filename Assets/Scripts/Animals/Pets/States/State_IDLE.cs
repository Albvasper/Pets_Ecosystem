using UnityEngine;

/// <summary>
/// Idle state for animals. Handles wandering behavior around pack leader or random positions.
/// Transitions to bumping state when colliding with other animals.
/// </summary>
public class State_IDLE : StateTypeAnimal
{
    const int randRangeX = 3;       // Random position in X
    const int randRangeY = 3;       // Random position in X
    const int cooldownMin = 1;      // Max IDLE cooldown 
    const int cooldownMax = 10;     // Min IDLE cooldown
    float cooldown;                 // Stores IDLE cooldown
    float counter;
    Vector2 target;                 // Random position

    public State_IDLE(BaseAnimal _animal) : base(_animal) { }

    public override void OnStateEnter()
    {
        // First time move position instantly
        cooldown = 0f;
        counter = cooldown;
        animal.BreedingPartner = null;
    }

    // TODO: Check if that path is valid if not look for one
    public override void Tick()
    {
        counter += Time.deltaTime;
        // IDLE movement logic
        if (counter >= cooldown)
        {
            // IDLE around pack leader is there is one, if not wander around
            if (animal.PackLeader != null && !animal.PackLeader.isDead)
            {
                target.x = animal.PackLeader.transform.position.x + Random.Range(-randRangeX, randRangeX);
                target.y = animal.PackLeader.transform.position.y + Random.Range(-randRangeY, randRangeY);
                cooldown = Random.Range(cooldownMin, cooldownMax);
                animal.Behavior.Walk(target);
                counter = 0;
            }
            else
            {
                target.x = animal.transform.position.x + Random.Range(-randRangeX, randRangeX);
                target.y = animal.transform.position.y + Random.Range(-randRangeY, randRangeY);
                cooldown = Random.Range(cooldownMin, cooldownMax);
                animal.Behavior.Walk(target);
                counter = 0;
            }
        }
        // Transition to bumping state
        if (animal.Physics.IsTouchingAgent)
        {
            BaseAnimal bumpingAnimal = animal.Physics.BumpingAnimal;
            if (bumpingAnimal == null || bumpingAnimal.isDead) return;

            // Filter if its a passive pet or a hostile pet
            if (animal.Friendliness == Friendliness.Hostile)
                animal.Behavior.SetState(new State_HostileBumping((HostilePet)animal, bumpingAnimal));
            else
                animal.Behavior.SetState(new State_Bumping((Pet)animal, bumpingAnimal));
        }
    }
}
 