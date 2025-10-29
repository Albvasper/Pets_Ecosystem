using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;

public enum TypeOfPet { Cat, Dog, Deer, Wolf, Bear, Tiger }
public enum Friendliness { Passive, Hostile }
public enum Sex { Male, Female }

/// <summary>
/// Base class for every pet. It connects BaseBehavior, BaseAnimator 
/// and BasePhysics classes. These 4 scripts manages every aspect of 
/// a pet.
/// Requires a BaseBehavior, BaseAnimator, BasePhysics, rigidbody, 
/// 2D collider and a navmesh components.
/// </summary>

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class BaseAnimal : MonoBehaviour
{
    const float breedingCooldown = 30f;
    public const float BreedingChance = 1f; //100%
    /// <summary>
    /// Pet name that will be displayed on their nametag.
    /// </summary>
    public string petName;
    /// <summary>
    /// ID that is unique to every pet. This helps track the pet and be able to remove them 
    /// form database safely.
    /// </summary>
    public string petID;
    /// <summary>
    /// Stores the pet species.
    /// Cat, Dog, Deer, Wolf, Bear or Tiger
    /// </summary>
    public TypeOfPet TypeOfPet;
    /// <summary>
    /// Stores if a pet is friendly by nature or not.
    /// If a pet is not friendly it will attack others.
    /// </summary>
    public Friendliness Friendliness;
    /// <summary>
    /// Stores if a pet is male or female for breeding system.
    /// </summary>
    public Sex Sex;
    /// <summary>
    /// Other animal of the same species and opposite sex that 
    /// this animal will mate with.
    /// </summary>
    public BaseAnimal BreedingPartner { get; set; }
    /// <summary>
    /// An animal that this animal will follow.
    /// </summary>
    public BaseAnimal PackLeader { get; set; }
    /// <summary>
    /// Class that handles the behavior of the pet
    /// </summary>
    public BaseBehavior Behavior { get; protected set; }
    /// <summary>
    /// Class that handles the physics of the pet
    /// </summary>
    public BasePhysics Physics { get; protected set; }
    /// <summary>
    /// Class that handles animation of the pet
    /// </summary>
    public BaseAnimator Animator { get; protected set; }
    public Rigidbody2D Rb2D { get; protected set; }
    public NavMeshAgent Agent { get; protected set; }
    public Collider2D Collider { get; protected set; }
    /// <summary>
    /// Text mesh pro text that displays the pets name
    /// </summary>
    public TextMeshProUGUI nameTag;
    public bool IsZombie { get; set; }
    public bool CanHaveKids;
    /// <summary>
    /// Stores the current happiness of the pet.
    /// From 0 (min) to a 1 (max).
    /// </summary>
    public float Happiness { get; set; }
    /// <summary>
    /// Stores the current sentience of the pet.
    /// From 0 (min) to a 1 (max).
    /// </summary>
    public float Sentience { get; set; }
    public int HP { get; protected set; }
    public bool isDead = false;
    /// <summary>
    /// Time it takes for the corpse to dissapear and delete the game object.
    /// </summary>
    [SerializeField] float corpseLifetime = 2f;
    [SerializeField] bool disablePhysicsOnDeath = true;
    [SerializeField] bool disableCollidersOnDeath = true;
    /// <summary>
    /// Stores the pet prefab that will spawn when this pet gives birth
    /// </summary>
    [SerializeField] protected GameObject Baby;
    protected int maxHp;
    float minLifetime = 240f;   // Minium time a pet will take a hit because of hunger
    float maxLifetime = 360f;   // Maximum time a pet will take a hit because of hunger
    float lifetime;             // Stores the time it will take for the pet to take a hit. 
    float zombieChance = 0.01f; // 1% chance of becoming a zombie.
    float counter;              // Counter for breeding cooldown.

    protected virtual void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Rb2D = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();
        Happiness = 50f;
        Sentience = 10f;
        CanHaveKids = false;
        IsZombie = false;
    }
    
    protected virtual void Start()
    {
        SetMaxHP();
        HP = maxHp;
        lifetime = Random.Range(minLifetime, maxLifetime);
        // Set the animal sex randomly
        Sex = Random.value < 0.5f ? Sex.Male : Sex.Female;
        // Add this to the animals list
        Pet_Manager.Instance.Pets.Add(this);
        Pet_Manager.Instance.RegisterBirth();
        // Set IDLE state as the default state
        Behavior.SetState(new State_IDLE(this));
        // Add animal to population count
        switch (TypeOfPet)
        {
            case TypeOfPet.Dog: Pet_Manager.Instance.AddToDogPopulation(); break;
            case TypeOfPet.Cat: Pet_Manager.Instance.AddToCatPopulation(); break;
            case TypeOfPet.Deer: Pet_Manager.Instance.AddToDeerPopulation(); break;
            case TypeOfPet.Wolf: Pet_Manager.Instance.AddToWolfPopulation(); break;
            case TypeOfPet.Tiger: Pet_Manager.Instance.AddToTigerPopulation(); break;
            case TypeOfPet.Bear: Pet_Manager.Instance.AddToBearPopulation(); break;
        }
        // 1% chance to be zombie on spawn
        if (Random.value <= zombieChance)
        {
            Animator.TurnIntoZombie();
        }
    }

    protected virtual void Update()
    {
        // When lifetime gets to 0: Take a hit
        lifetime -= Time.deltaTime;
        if (lifetime <= 0 && !isDead)
        {
            TakeDamage();
            lifetime = maxLifetime;
        }
        if (CanHaveKids == false && !isDead)
        {
            counter += Time.deltaTime;
            if (counter >= breedingCooldown)
            {
                CanHaveKids = true;
                counter = 0;
            }
        }
    }

    public void SetPetID(string id)
    {
        petID = id;
    }

    public void SetPetName(string _name)
    {
        name = _name;
        nameTag.text = _name;
    }

    //TODO: MOVE THIS TO THE BASE BEHAVIOR CLASS
    /// <summary>
    /// Spawn the baby prefab when the breeding process is over.
    /// </summary>
    /// <param name="breedingPartner">Parent of the child.</param>
    public virtual void GiveBirth(BaseAnimal breedingPartner)
    {
        PackLeader = breedingPartner;
        Vector2 midPoint = (transform.position + breedingPartner.transform.position) / 2f;
        BaseAnimal b = Instantiate(Baby, midPoint, Quaternion.identity).GetComponent<BaseAnimal>();
        b.PackLeader = breedingPartner;
        b.Behavior.AddSentience(11);
    }

    protected virtual void SetMaxHP()
    {
        maxHp = 3;
    }

    public void Heal()
    {
        if (HP < maxHp)
            HP--;
    }

    public void TakeDamage()
    {
        HP--;
        if (HP <= 0)
            Die();
    }

    private void HandleCorpsePhysics()
    {
        if (Rb2D != null && disablePhysicsOnDeath)
        {
            Rb2D.bodyType = RigidbodyType2D.Kinematic;
            Rb2D.linearVelocity = Vector3.zero;
        }
        
        if (disableCollidersOnDeath)
        {
            Collider.enabled = false;
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        // Turn of colliders and nav mesh
        Agent.enabled = false;
        HandleCorpsePhysics();
        Animator.DeathAnimation();
        if (gameObject.activeSelf)
            StartCoroutine(CleanUpCorpse());
        Pet_Manager.Instance.Pets.Remove(this);
        // Remove pet from population
        switch (TypeOfPet)
        {
            case TypeOfPet.Dog: Pet_Manager.Instance.RemoveFromDogPopulation(); break;
            case TypeOfPet.Cat: Pet_Manager.Instance.RemoveFromCatPopulation(); break;
            case TypeOfPet.Deer: Pet_Manager.Instance.RemoveFromDeerPopulation(); break;
            case TypeOfPet.Wolf: Pet_Manager.Instance.RemoveFromWolfPopulation(); break;
            case TypeOfPet.Tiger: Pet_Manager.Instance.RemoveFromTigerPopulation(); break;
            case TypeOfPet.Bear: Pet_Manager.Instance.RemoveFromBearPopulation(); break;
        }

        if (string.IsNullOrEmpty(petID)) return;
        // Remove pet from database
        if (FirebaseREST.Instance != null)
        {
            FirebaseREST.Instance.DeleteData($"ecosystem/pets/{petID}", success =>
            {
                if (success)
                    Debug.Log($"Deleted pet {petName} ({petID}) from Firebase");
                else
                    Debug.LogWarning($"Failed to delete pet {petName} ({petID})");
            });
        }
        else
        {
            Debug.LogWarning("FirebaseREST.Instance is null — cannot delete pet from DB");
        }
    }

    IEnumerator CleanUpCorpse()
    {
        yield return new WaitForSeconds(corpseLifetime);
        yield return StartCoroutine(FadeOutCorpse());
        Destroy(gameObject);
    }

    private IEnumerator FadeOutCorpse(float duration = 1f)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = 1f - (elapsed / duration);

            foreach (var renderer in renderers)
            {
                foreach (var mat in renderer.materials)
                {
                    if (mat.HasProperty("_Color"))
                    {
                        Color color = mat.color;
                        color.a = alpha;
                        mat.color = color;
                    }
                }
            }
            yield return null;
        }
    }
}