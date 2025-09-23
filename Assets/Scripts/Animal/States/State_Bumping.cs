using UnityEngine;

public class State_Bumping : State
{
    float counter = 0f;
    const float pushingForce = 2f;
    GameObject otherAnimal;

    public State_Bumping(Animal _animal, GameObject other) : base(_animal)
    {
        this.otherAnimal = other;
    }

    public override void OnStateEnter()
    {
        Debug.Log("PUSHING!");
        animal.GetAnimalPhysics().PushAnimal(otherAnimal, pushingForce);
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
        if (counter >= animal.GetBumpingCooldown())
        {
            animal.GetAnimalBehavior().SetState(new State_IDLE(animal));
            counter = 0;
        }
    }

    public override void OnStateExit()
    {
        Debug.Log("BACK TO NORMAL!");
        animal.GetAnimalPhysics().IsNotTouchingAgent();
        animal.GetAnimalPhysics().CleanUpAfterPush();
    }
}
