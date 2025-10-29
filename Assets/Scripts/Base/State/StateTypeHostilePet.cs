/// <summary>
/// Base state class for states that only hostile pets use.
/// </summary>

public abstract class StateTypeHostilePet : State
{
    protected HostilePet pet;

    public StateTypeHostilePet(HostilePet _pet)
    {
        pet = _pet;
    }
}
