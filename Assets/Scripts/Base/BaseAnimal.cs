    using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BaseBehavior))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseAnimal : MonoBehaviour
{
    protected AnimalData animalData;
    public BaseBehavior Behavior { get; protected set; }
    public Rigidbody2D Rb2D { get; protected set; }
    public NavMeshAgent Agent { get; protected set; }
    public const float BumpingCooldown = 3f;
    protected float counter = BumpingCooldown;

    protected virtual void Awake()
    {
        Behavior = GetComponent<BaseBehavior>();
        Agent = GetComponent<NavMeshAgent>();
        Rb2D = GetComponent<Rigidbody2D>();
    }
}
