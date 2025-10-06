using UnityEngine;

public abstract class State
{
    public virtual void OnStateEnter() {}
    public abstract void Tick();
    public virtual void OnStateExit() {}
}
