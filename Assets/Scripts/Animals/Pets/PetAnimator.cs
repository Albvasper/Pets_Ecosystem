using UnityEngine;

public class PetAnimator : BaseAnimator
{
    Pet pet;

    protected override void Awake()
    {
        base.Awake();
        pet = GetComponent<Pet>();
    }

    protected virtual void Update()
    {
        AnimatePet();
    }
    
    void AnimatePet()
    {
        Vector2 vel = pet.Agent.velocity;
        bool isMoving = vel.magnitude > MoveThreshold;
        // Sprite direction
        if (isMoving)
        {
            lastVelocity = vel;
            Animator.SetBool("IsMoving", true);
            Animator.SetFloat("InputX", vel.x);
            Animator.SetFloat("InputY", vel.y);
        }
        else
        {
            Animator.SetBool("IsMoving", false);
            Animator.SetFloat("LastInputX", lastVelocity.x);
            Animator.SetFloat("LastInputY", lastVelocity.y);
        }

        // Shadow logic
        Vector2 dir = lastVelocity.normalized;
        if (Mathf.Abs(dir.x) < 0.01f) dir.x = 0f;
        if (Mathf.Abs(dir.y) < 0.01f) dir.y = 0f;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            spriteRenderer.sprite = shadowHorizontal;
        }
        else
        {
            spriteRenderer.sprite = shadowVertical;
        }

        // Stunned logic
        if (IsBeingBumped)
        {
            Animator.SetBool("IsStunned", true);
            spriteRenderer.sprite = shadowVertical;
        }
        else
        {
            Animator.SetBool("IsStunned", false);
        }

        if (pet.isDead)
            spriteRenderer.sprite = shadowHorizontal;
    }
}



