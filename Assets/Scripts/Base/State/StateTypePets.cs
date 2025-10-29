/// <summary>
/// Base state class for states that only peaceful pets use.
/// </summary>
public abstract class StateTypePets : State
{
    protected Pet pet;

    public StateTypePets(Pet _pet)
    {
        pet = _pet;
    }
}
