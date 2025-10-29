/// <summary>
/// Base class for state machine states.
/// Inherit from this class to create custom states with enter, update, and exit logic.
/// </summary>
public abstract class State
{
    /// <summary>
    /// Called once when entering a state.
    /// </summary>
    public virtual void OnStateEnter() { }
    /// <summary>
    /// Called every frame on loop.
    /// </summary>
    public abstract void Tick();
    /// <summary>
    /// Called once when exiting a state.
    /// </summary>
    public virtual void OnStateExit() { }
}
