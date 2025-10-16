public abstract class StateTypePets : State
{
    protected Pet pet;

    public StateTypePets(Pet _pet)
    {
        pet = _pet;
    }
}
