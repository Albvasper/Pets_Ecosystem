using UnityEngine;

/// <summary>
/// Breeding state for animals. Handles walking to each other and mating for a couple of seconds,
/// then spawns a baby animal and updates sentience and happiness.
/// Transitions to IDLE state when finishig mating.
/// </summary>
public class State_Breeding : StateTypeAnimal
{
    const float timeForBreeding = 6f;   // Time it takes to finish breeding process.
    float counter;                      // Counter for breeding process.
    BaseAnimal otherAnimal;

    public State_Breeding(BaseAnimal _animal, BaseAnimal _otherAnimal) : base(_animal)
    {
        otherAnimal = _otherAnimal;
    }

    public override void Tick()
    {
        if (otherAnimal == null || otherAnimal.isDead)
        {
            // Abort breeding
            animal.BreedingPartner = null;
            animal.Behavior.SetState(new State_IDLE(animal));
            return;
        }
        counter += Time.deltaTime;
        animal.Behavior.Walk(otherAnimal.transform.position);

        if (counter >= timeForBreeding)
        {
            if (animal.Sex == Sex.Female)
            {
                animal.GiveBirth(otherAnimal);
                animal.Behavior.AddSentience(5);
            }
            animal.Behavior.AddHappiness(15);
            animal.Behavior.AddSentience(10);
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
