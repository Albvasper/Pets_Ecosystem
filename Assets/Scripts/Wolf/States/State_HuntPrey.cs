using UnityEngine;

public class State_HuntPrey : State
{
    GameObject otherAnimal;

    public State_HuntPrey(Wolf _wolf, GameObject other) : base(_wolf)
    { 
        this.otherAnimal = other;
    }

    public override void OnStateEnter()
    {
        wolf.Hunting = true;
    }

    public override void Tick()
    {
        wolf.WolfBehavior.Walk(otherAnimal.transform.position);
    }
}
