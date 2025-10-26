using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BaseAnimal))]
public abstract class BaseAnimator : MonoBehaviour
{
    protected BaseAnimal animal;

    public RuntimeAnimatorController ZombieAnimator;
    
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite shadowHorizontal;
    [SerializeField] protected Sprite shadowVertical;
    public bool IsBeingBumped { get; set; }
    public Animator Animator { get; private set; }
    protected Vector2 lastVelocity;
    protected const float MoveThreshold = 0.05f;

    protected virtual void Awake()
    {
        animal = GetComponent<BaseAnimal>();
        Animator = GetComponent<Animator>();
        IsBeingBumped = false;
    }

    public void TurnIntoZombie()
    {
        animal.IsZombie = true;
        Pet_Manager.Instance.AddToZombiePopulation();
        Animator.runtimeAnimatorController = ZombieAnimator;
    }
    
    public void DeathAnimation()
    {
        Animator.SetBool("IsDead", true);
    }
}
