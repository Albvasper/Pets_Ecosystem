using UnityEngine;

/// <summary>
/// Base class that handles all pets animations like IDLE, walking, bumping, etc.
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BaseAnimal))]
public abstract class BaseAnimator : MonoBehaviour
{
    public const string IsMoving = "IsMoving";
    public const string InputX = "InputX";
    public const string InputY = "InputY";
    public const string LastInputX = "LastInputX";
    public const string LastInputY = "LastInputY";
    public const string IsStunned = "IsStunned";
    /// <summary>
    /// 
    /// </summary>
    protected const float MoveThreshold = 0.05f;
    public Animator Animator { get; private set; }
    /// <summary>
    /// Animator that stores all the animations in zombie style.
    /// </summary>
    public RuntimeAnimatorController ZombieAnimator;
    /// <summary>
    /// Indicates if the pet is currently touching another pet
    /// </summary>
    public bool IsBeingBumped { get; set; }
    [SerializeField] protected SpriteRenderer spriteRenderer;
    /// <summary>
    /// Stores pet shadow sprite in horizontal form
    /// </summary>
    [SerializeField] protected Sprite shadowHorizontal;
    /// <summary>
    /// Stores pet shadow sprite in vertical form
    /// </summary>
    [SerializeField] protected Sprite shadowVertical;
    protected BaseAnimal animal;
    protected Vector2 lastVelocity;

    protected virtual void Awake()
    {
        animal = GetComponent<BaseAnimal>();
        Animator = GetComponent<Animator>();
        IsBeingBumped = false;
    }
    
    public void TurnIntoZombie()
    {
        if (ZombieAnimator != null)
            Animator.runtimeAnimatorController = ZombieAnimator;
        else
            Debug.LogWarning("Zombie animator is null!");
    }

    public void DeathAnimation()
    {
        Animator.SetBool("IsDead", true);
    }

    protected void UpdateMovementAnimation(Vector2 velocity, bool isMoving)
    {
        Animator.SetBool(IsMoving, isMoving);
    
        if (isMoving)
        {
            lastVelocity = velocity;
            Animator.SetFloat(InputX, velocity.x);
            Animator.SetFloat(InputY, velocity.y);
        }
        else
        {
            Animator.SetFloat(LastInputX, lastVelocity.x);
            Animator.SetFloat(LastInputY, lastVelocity.y);
        }
    }

    protected void UpdateStunnedAnimation()
    {
        Animator.SetBool(IsStunned, IsBeingBumped);
    }

    protected void UpdateShadowSprite(BaseAnimal pet)
    {
        if (pet.isDead)
        {
            spriteRenderer.sprite = shadowHorizontal;
            return;
        }

        if (IsBeingBumped)
        {
            spriteRenderer.sprite = shadowVertical;
            return;
        }

        spriteRenderer.sprite = GetShadowSpriteFromDirection(lastVelocity);
    }

    /// <summary>
    /// Calculates the shadow direction for pets.
    /// </summary>
    /// <param name="velocity">Pet current or last velocity.</param>
    /// <returns>Shadow sprite with the correct direction.</returns>
    Sprite GetShadowSpriteFromDirection(Vector2 velocity)
    {
        Vector2 direction = velocity.normalized;
        
        // Filter out noise from near-zero values
        if (Mathf.Abs(direction.x) < 0.01f) direction.x = 0f;
        if (Mathf.Abs(direction.y) < 0.01f) direction.y = 0f;
        
        return Mathf.Abs(direction.x) > Mathf.Abs(direction.y) 
            ? shadowHorizontal 
            : shadowVertical;
    }
}
