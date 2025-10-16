using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class BaseAnimator : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite shadowHorizontal;
    [SerializeField] protected Sprite shadowVertical;
    public bool IsBeingBumped { get; set; }
    public Animator Animator { get; private set; }
    protected Vector2 lastVelocity;
    protected const float MoveThreshold = 0.05f;

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        IsBeingBumped = false;
    }
}
