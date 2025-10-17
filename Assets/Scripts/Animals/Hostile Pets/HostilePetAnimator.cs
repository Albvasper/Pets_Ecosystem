using UnityEngine;

[RequireComponent(typeof(HostilePet))]
public class HostilePetAnimator : PetAnimator
{
    HostilePet pet;

    protected override void Awake()
    {
        base.Awake();
        pet = GetComponent<HostilePet>();
    }

    protected override void Update()
    {
        base.Update();
        if (pet.IsAttacking)
        {
            Animator.SetBool("IsAttacking", true);
        }
        else
        {
            Animator.SetBool("IsAttacking", false);
        }
    }
}
