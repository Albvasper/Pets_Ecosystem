using UnityEngine;

public class HostilePetAnimator : PetAnimator
{
    private HostilePet HostilePet => animal as HostilePet;

    protected override void Awake()
    {
        base.Awake();
        if (animal == null)
            animal = GetComponent<HostilePet>();
    }

    protected override void Update()
    {
        base.Update();
        if (HostilePet != null)
        {
            if (HostilePet.IsAttacking)
            {
                Animator.SetBool("IsAttacking", true);
            }
            else
            {
                Animator.SetBool("IsAttacking", false);
            }
        }
    }
}
