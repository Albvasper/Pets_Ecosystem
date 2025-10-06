using UnityEngine;

public abstract class StateTypeWolf : State
{
    protected Wolf wolf;

    public StateTypeWolf(Wolf _wolf)
    {
        wolf = _wolf;
    }
}
