using UnityEngine;

public abstract class StateTypeAnimal : State
{
    protected BaseAnimal animal;

    public StateTypeAnimal(BaseAnimal _animal)
    {
        animal = _animal;
    }
}
