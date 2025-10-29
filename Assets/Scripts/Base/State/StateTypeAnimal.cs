/// <summary>
/// Base state class for the type of states that all animals use.
/// </summary>
public abstract class StateTypeAnimal : State
{
    protected BaseAnimal animal;

    public StateTypeAnimal(BaseAnimal _animal)
    {
        animal = _animal;
    }
}
