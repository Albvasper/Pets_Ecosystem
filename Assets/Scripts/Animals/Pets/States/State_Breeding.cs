using UnityEngine;

public class State_Breeding : StateTypeAnimal
{
    BaseAnimal otherAnimal;
    float counter;
    const float timeForBreeding = 6f;

    public State_Breeding(BaseAnimal _animal, BaseAnimal _otherAnimal) : base(_animal)
    {
        otherAnimal = _otherAnimal;
    }

    public override void OnStateEnter()
    {
        Debug.Log("START BREEDING");
        animal.AllowBumping = false;
    }

    public override void Tick()
    {
        counter += Time.deltaTime;
        animal.Behavior.Walk(otherAnimal.transform.position);
        if (counter >= timeForBreeding)
        {
            animal.GiveBirth(otherAnimal);
            animal.Behavior.SetState(new State_IDLE(animal));
        }
    }

    public override void OnStateExit()
    {
        Debug.Log("EXITING BREEDING");
        animal.BreedingPartner = null;
        animal.AllowBumping = true;
    }
}
