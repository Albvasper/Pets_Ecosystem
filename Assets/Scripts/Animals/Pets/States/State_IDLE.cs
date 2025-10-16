using UnityEngine;

public class State_IDLE : StateTypeAnimal
{
    // Random pos
    const int randRangeX = 3;
    const int randRangeY = 3;
    Vector2 target;
    // Timer
    const int cooldownMax = 1;
    const int cooldownMin = 10;
    float cooldown;
    float counter;

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
        // IDLE movement logic
        counter += Time.deltaTime;
        if (counter >= cooldown)
        {
            // IDLE around pack leader is there is one, if not wander around
            if (animal.PackLeader)
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
            // Filter if its a passive pet or a hostile pet
            if (animal.Friendliness == Friendliness.Hostile)
            {
                animal.Behavior.SetState(new State_HostileBumping((HostilePet)animal, animal.Physics.BumpingAnimal));
            }
            else
            {
                animal.Behavior.SetState(new State_Bumping((Pet)animal, animal.Physics.BumpingAnimal));
            }
        }
    }
}
 