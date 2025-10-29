using UnityEngine;

/// <summary>
/// Base class for all animals that handles collisions, physics and pathfinfing.
/// </summary>
public abstract class BasePhysics : MonoBehaviour
{
    protected const float linearDamping = 2f;
    /// <summary>
    /// Stores a pet that the current pet is touching.
    /// </summary>
    public BaseAnimal BumpingAnimal { get; set; }
    /// <summary>
    /// Indicates if the pet is touching anther one
    /// </summary>
    public bool IsTouchingAgent { get; set; }

    protected virtual void Awake()
    {
        IsTouchingAgent = false;
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Animal"))
        {
            IsTouchingAgent = false;
            BumpingAnimal = null;
        }
    }

    /// <summary>
    /// Push character in a certain direction.
    /// </summary>
    /// <param name="animal">Animal that will be pushed.</param>
    /// <param name="otherAnimal">Oppostie direction that the animal will be pushed.</param>
    /// <param name="force">Pushing force.</param>
    public virtual void PushAnimal(BaseAnimal animal, BaseAnimal otherAnimal, float force)
    {
        animal.Behavior.StopWalking();
        animal.Rb2D.gravityScale = 0;
        animal.Rb2D.bodyType = RigidbodyType2D.Dynamic;
        animal.Rb2D.linearDamping = linearDamping;
        Vector3 direction = animal.transform.position - otherAnimal.transform.position;
        animal.Rb2D.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Reset values post pushing animal
    /// </summary>
    /// <param name="animal">Pet that will be cleaned up.</param>
    public virtual void CleanUpAfterPush(BaseAnimal animal)
    {
        animal.Rb2D.linearVelocity = Vector2.zero;
        animal.Rb2D.angularVelocity = 0f;
        animal.Rb2D.bodyType = RigidbodyType2D.Kinematic;
        animal.Agent.Warp(transform.position);
        animal.Behavior.KeepWalking();
    }
}
