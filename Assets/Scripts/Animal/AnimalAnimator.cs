using NUnit.Framework;
using UnityEngine;

public class AnimalAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer currentShadow;
    [SerializeField] private Sprite shadowHorizontal; 
    [SerializeField] private Sprite shadowVertical;
    public bool isBeingBumped { get; set; }
    public Animator animator { get; private set; }
    Animal animal;
    Vector2 lastVelocity;
    const float MoveThreshold = 0.05f;

    void Awake()
    {
        animal = GetComponent<Animal>();
        animator = GetComponent<Animator>();
        isBeingBumped = false;
    }

    void Update()
    {
        Vector2 vel = animal.Agent.velocity;
        bool isMoving = vel.magnitude > MoveThreshold;
        // Sprite direction
        if (isMoving)
        {
            lastVelocity = vel;
            animator.SetBool("IsMoving", true);
            animator.SetFloat("InputX", vel.x);
            animator.SetFloat("InputY", vel.y);
        }
        else
        {
            animator.SetBool("IsMoving", false);
            animator.SetFloat("LastInputX", lastVelocity.x);
            animator.SetFloat("LastInputY", lastVelocity.y);
        }

        // Shadow logic
        Vector2 dir = lastVelocity.normalized;
        if (Mathf.Abs(dir.x) < 0.01f) dir.x = 0f;
        if (Mathf.Abs(dir.y) < 0.01f) dir.y = 0f;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            currentShadow.sprite = shadowHorizontal;
        }
        else
        {
            currentShadow.sprite = shadowVertical;
        }

        // Stunned logic
        if (isBeingBumped)
        {
            animator.SetBool("IsStunned", true);
            currentShadow.sprite = shadowVertical;
        }
        else
        {
            animator.SetBool("IsStunned", false);
        }
    }
}
