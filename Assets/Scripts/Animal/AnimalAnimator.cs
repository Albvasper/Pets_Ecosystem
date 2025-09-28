using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class AnimalAnimator : MonoBehaviour
{
    Animal animal;
    public Animator animator { get; private set; }
    Vector2 lastVelocity;
    const float MoveThreshold = 0.05f;

    void Awake()
    {
        animal = GetComponent<Animal>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 vel = animal.Agent.velocity;
        bool isMoving = vel.magnitude > MoveThreshold;
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
    }
}
