using UnityEngine;

/// <summary>
/// Updates peaceful pet animation and sprite rendering based on current state.
/// Called every frame to synchronize visuals.
/// </summary>
public class PetAnimator : BaseAnimator
{
    Pet Pet => animal as Pet;

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Update()
    {
        if (Pet == null) return;
        Vector2 velocity = Pet.Agent.velocity;
        bool isMoving = velocity.magnitude > MoveThreshold;

        UpdateMovementAnimation(velocity, isMoving);
        UpdateStunnedAnimation();
        UpdateShadowSprite(Pet);
    }
}