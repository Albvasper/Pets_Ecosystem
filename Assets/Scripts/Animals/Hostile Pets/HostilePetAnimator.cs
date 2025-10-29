using UnityEngine;

/// <summary>
/// Updates hostile pet animation and sprite rendering based on current state.
/// Called every frame to synchronize visuals.
/// </summary>
public class HostilePetAnimator : PetAnimator
{
    HostilePet HostilePet => animal as HostilePet;
    public const string IsAttacking = "IsAttacking";

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        if (HostilePet == null) return;
        Animator.SetBool(IsAttacking, HostilePet.IsAttacking);
    }
}
