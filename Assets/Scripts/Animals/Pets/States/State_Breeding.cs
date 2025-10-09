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

    public override void Tick()
    {
        counter += Time.deltaTime;
        animal.Behavior.Walk(otherAnimal.transform.position);
        if (counter >= timeForBreeding)
        {
            if (animal.Sex == Sex.Female)
            {
                animal.GiveBirth(otherAnimal);
            }
            animal.Behavior.SetState(new State_IDLE(animal));
            counter = 0;
        }
    }

    public override void OnStateExit()
    {
        animal.CanHaveKids = false;
        animal.BreedingPartner = null;
    }
}
