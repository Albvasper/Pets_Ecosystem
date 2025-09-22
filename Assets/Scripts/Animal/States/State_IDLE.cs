using UnityEngine;

public class State_IDLE : State
{
    const int randRangeX = 5;
    const int randRangeY = 5;
    int targetX, targetY;
    Vector2 target;
    float counter = 0f;
    const float limit = 5f;

    public State_IDLE(Animal _animal) : base(_animal) { }

    // Behavior
    /*  IDLE:
        - look for a random point on a certain range
        - check if that point is valid
        - move the animal there
        - wait for a couple of seconds
    */
    // TODO: Make the wait time random between a range and also take the current pos and then add or substract the random pos
    public override void Tick()
    {
        counter += Time.deltaTime;
        if (counter >= limit)
        {
            target.x = Random.Range(0, randRangeX);
            target.y = Random.Range(0, randRangeY);
            animal.animalBehavior.Walk(target);
            counter = 0;
        }
    }
}
 