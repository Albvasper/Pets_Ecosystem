using UnityEngine;

/// <summary>
/// Class that manages all the funtionalities of a peaceful pet.
/// Inherits form BaseAnimal and requires PetBehavior, PetAnimator, 
/// PetPhysics, rigidbody, 2D collider and a navmesh components.
/// </summary>
[RequireComponent(typeof(PetBehavior))]
[RequireComponent(typeof(PetPhysics))]
[RequireComponent(typeof(PetAnimator))]
public class Pet : BaseAnimal
{
    protected override void Awake()
    {
        base.Awake();
        Behavior = GetComponent<PetBehavior>();
        Physics = GetComponent<PetPhysics>();
        Animator = GetComponent<PetAnimator>();
    }

    protected virtual void OnEnable()
    {
        Friendliness = Friendliness.Passive;
    }
}
