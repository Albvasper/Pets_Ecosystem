/// <summary>
/// Hunt state for hostile pets. Walks up to the prey and transitions to attacking state.
/// Transitions to Idle and attacking state.
/// </summary>
public class State_Hunt : StateTypeHostilePet
{
    BaseAnimal prey;

    public State_Hunt(HostilePet _pet, BaseAnimal _prey) : base(_pet)
    {
        prey = _prey;
    }

    public override void OnStateEnter()
    {
        pet.IsHunting = true;
    }

    public override void Tick()
    {
        // Make sure prey exists and is alive
        if (prey == null || prey.isDead)
        {
            pet.Behavior.SetState(new State_IDLE(pet));
            return;
        }
        // If prey exists walk to it until it bumps with it.
        pet.Behavior.Walk(pet.CurrentPrey.gameObject.transform.position);
        if (pet.Physics.IsTouchingAgent)
        {
            pet.Behavior.SetState(new State_Attack(pet, prey));
        }
    }
}
