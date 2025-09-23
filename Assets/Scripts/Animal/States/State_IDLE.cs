using UnityEngine;

public class State_IDLE : State
{
    // Random pos
    const int randRangeX = 5;
    const int randRangeY = 5;
    int targetX, targetY;
    Vector2 target;
    // Timer
    const int cooldownMax = 1;
    const int cooldownMin = 10;
    float cooldown;
    float counter;

    public State_IDLE(Animal _animal) : base(_animal) { }

    public override void OnStateEnter()
    {
        // First time move position instantly
        cooldown = 0f;
        counter = cooldown;
    }

    // Behavior
    /*  IDLE:
        - look for a random point on a certain range
        - check if that point is valid
        - move the animal there
        - wait for a couple of seconds
    */
    public override void Tick()
    {
        counter += Time.deltaTime;
        if (counter >= cooldown)
        {
            // cuurent pos + 
            target.x = animal.transform.position.x + Random.Range(-randRangeX, randRangeX);
            target.y = animal.transform.position.y + Random.Range(-randRangeY, randRangeY);
            cooldown = Random.Range(cooldownMin, cooldownMax);
            animal.GetAnimalBehavior().Walk(target);
            counter = 0;
        }
    }
}
 