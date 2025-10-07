using System;
using UnityEngine;

public abstract class BasePhysics : MonoBehaviour
{
    public bool IsTouchingAgent { get; set; }
    public BaseAnimal BumpingAnimal { get; set; }
    protected const float linearDamping = 2f;

    protected virtual void Awake()
    {
        IsTouchingAgent = false;
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Animal")
        {
            IsTouchingAgent = false;
            BumpingAnimal = null;
        }
    }

    // Push character in a certain direction
    public virtual void PushAnimal(BaseAnimal animal, BaseAnimal otherAnimal, float force)
    {
        animal.Behavior.StopWalking();
        animal.Rb2D.gravityScale = 0;
        animal.Rb2D.bodyType = RigidbodyType2D.Dynamic;
        animal.Rb2D.linearDamping = linearDamping;
        Vector3 direction = animal.transform.position - otherAnimal.transform.position;
        animal.Rb2D.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }

    // Reset values post pushing animal
    public virtual void CleanUpAfterPush(BaseAnimal animal)
    {
        animal.Rb2D.linearVelocity = Vector2.zero;
        animal.Rb2D.angularVelocity = 0f;
        animal.Rb2D.bodyType = RigidbodyType2D.Kinematic;
        animal.Agent.Warp(transform.position);
        animal.Behavior.KeepWalking();
    }
}
