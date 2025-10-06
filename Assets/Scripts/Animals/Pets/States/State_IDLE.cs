using UnityEngine;

public class State_IDLE : StateTypeAnimal
{
    // Random pos
    const int randRangeX = 5;
    const int randRangeY = 5;
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
    }

    // Behavior
    /*  IDLE:
        - look for a random point on a certain range
        - check if that point is valid
        - move the animal there
        - wait for a couple of seconds
    */
    // TODO: Check if that path is valid if not look for one
    public override void Tick()
    {
        counter += Time.deltaTime;
        if (counter >= cooldown)
        {
            target.x = animal.transform.position.x + Random.Range(-randRangeX, randRangeX);
            target.y = animal.transform.position.y + Random.Range(-randRangeY, randRangeY);
            cooldown = Random.Range(cooldownMin, cooldownMax);
            animal.Behavior.Walk(target);
            counter = 0;
        }
    }
}
 