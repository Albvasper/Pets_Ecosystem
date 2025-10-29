using UnityEngine;

/// <summary>
/// Class that manages all the funtionalities of a hostile pet.
/// Inherits form Pet and requires HostilePetBehavior, HostilePetAnimator, 
/// HostilePetPhysics, rigidbody, 2D collider and a navmesh components.
/// </summary>
[RequireComponent(typeof(HostilePetBehavior))]
[RequireComponent(typeof(HostilePetPhysics))]
[RequireComponent(typeof(HostilePetAnimator))]
public class HostilePet : Pet
{
    public bool IsHunting { get; set; }
    public bool IsAttacking { get; set; }
    public BaseAnimal CurrentPrey { get; set; }

    protected override void Awake()
    {
        base.Awake();
        Behavior = GetComponent<HostilePetBehavior>();
        Physics = GetComponent<HostilePetPhysics>();
        Animator = GetComponent<HostilePetAnimator>();
        IsHunting = false;
        IsAttacking = false;
    }

    protected override void OnEnable()
    {
        Friendliness = Friendliness.Hostile;
    }

    // Set max hp to 5. Only for hostile pets
    protected override void SetMaxHP()
    {
        maxHp = 5; 
    }
}