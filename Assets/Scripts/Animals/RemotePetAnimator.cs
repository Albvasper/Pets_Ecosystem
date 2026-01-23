using UnityEngine;

public class RemotePetAnimator : MonoBehaviour
{
    const float MoveThreshold = 0.05f;
    Animator animator;
    Vector2 lastVelocity;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
    }  

    public void UpdateMovementAnimation(Vector2 velocity)
    {
        bool isMoving = velocity.sqrMagnitude > MoveThreshold * MoveThreshold;
        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            lastVelocity = velocity;
            animator.SetFloat("InputX", velocity.x);
            animator.SetFloat("InputY", velocity.y);
        }
        else
        {
            animator.SetFloat("LastInputX", lastVelocity.x);
            animator.SetFloat("LastInputY", lastVelocity.y);
        }
    }
}
