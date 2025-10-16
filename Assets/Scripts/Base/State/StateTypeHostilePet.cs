public abstract class StateTypeHostilePet : State
{
    protected HostilePet pet;

    public StateTypeHostilePet(HostilePet _pet)
    {
        pet = _pet;
    }
}
