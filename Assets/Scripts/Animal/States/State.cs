using UnityEngine;

public abstract class State
{
    protected Animal animal;
    protected Wolf wolf;
    protected Animal prey;

    public State(Animal _animal)
    {
        animal = _animal;
    }

    public State(Wolf _wolf)
    {
        wolf = _wolf;
    }

    public State(Wolf _wolf, Animal _prey)
    {
        wolf = _wolf;
        prey = _prey;
    }
    public virtual void OnStateEnter() { }
    public abstract void Tick();
    public virtual void OnStateExit(){}
}
