using UnityEngine;

[RequireComponent(typeof(BaseBehavior))]
[RequireComponent(typeof(AnimalPhysics))]
[RequireComponent(typeof(AnimalAnimator))]
public class Animal : BaseAnimal
{
    protected override void Awake()
    {
        base.Awake();
        Physics = GetComponent<AnimalPhysics>();
        Animator = GetComponent<AnimalAnimator>();
    }
}