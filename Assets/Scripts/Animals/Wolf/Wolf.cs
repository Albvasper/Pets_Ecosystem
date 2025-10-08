using UnityEngine;

[RequireComponent(typeof(BaseBehavior))]
[RequireComponent(typeof(WolfPhysics))]
[RequireComponent(typeof(WolfAnimator))]
public class Wolf : BaseAnimal
{
    public bool IsHunting { get; set; }
    public bool IsAttacking { get; set; }
    public Animal CurrentPrey { get; set;}

    protected override void Awake()
    {
        base.Awake();
        Physics = GetComponent<WolfPhysics>();
        Animator = GetComponent<WolfAnimator>();
        IsHunting = false;
        IsAttacking = false;
    }
}
