using UnityEngine;

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
