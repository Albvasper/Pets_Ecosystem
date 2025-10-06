using UnityEngine;

public abstract class StateTypePets : State
{
    protected Animal animal;

    public StateTypePets(Animal _animal)
    {
        animal = _animal;
    }
}
