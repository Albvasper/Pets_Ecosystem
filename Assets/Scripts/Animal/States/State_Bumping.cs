using UnityEngine;

public class State_Bumping : State
{
    float counter = 0f;
    const float limit = 4f;
    const float pushingForce = 0.05f;
    GameObject otherAnimal;

    public State_Bumping(Animal _animal, GameObject other) : base(_animal)
    {
        this.otherAnimal = other;
    }

    public override void OnStateEnter()
    {
        if (otherAnimal != null) {
            animal.animalPhysics.PushAnimal(otherAnimal, pushingForce);
        }
        else
            Debug.Log("There is no animal!");
    }

    // Behavior
    /*  Dizzy:
        - Make a dizzy animation
        - Wait 4 seconds
        - Go back to IDLE
    */
    public override void Tick()
    {
        counter += Time.deltaTime;
        if (counter >= limit)
        {
            animal.animalBehavior.SetState(new State_IDLE(animal));
            counter = 0;
        }
    }

    public override void OnStateExit()
    {
        animal.animalPhysics.IsNotTouchingAgent();
        animal.animalPhysics.CleanUpAfterPush();
    }
}
