using UnityEngine;

public abstract class State
{
    protected Animal animal;

    public State(Animal _animal)
    {
        animal = _animal;
    }

    public virtual void OnStateEnter(){}
    public abstract void Tick();
    public virtual void OnStateExit(){}
}
