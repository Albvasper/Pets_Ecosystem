using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public class AnimalPhysics : MonoBehaviour
{
    [SerializeField] public float rbMass = 100f;
    [SerializeField] public float rbLinearDamping = 2f;
    public Animal animal;
    bool isTouchingAgent;
    Rigidbody2D parentRB;
    CircleCollider2D parentCollider;
    GameObject bumpingAnimal;

    void Awake()
    {
        isTouchingAgent = false;
        animal = transform.parent.GetComponent<Animal>();
        parentRB = animal.GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    // Hanldes collision with other Game Objects
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Animal")
        {
            isTouchingAgent = true;
            bumpingAnimal = collision.gameObject;
        }

    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Animal")
        {
            isTouchingAgent = false;
            bumpingAnimal = null;
        }

    }

    // Push character in a certain direction adding a temp collider and rb
    public void PushAnimal(GameObject otherAnimal, float force)
    {
        // Attach a rb to the animal
        if (parentRB == null)
            parentRB = animal.gameObject.AddComponent<Rigidbody2D>();
        // Attach a collider to the animal
        if (parentCollider == null)
            parentCollider = animal.gameObject.AddComponent<CircleCollider2D>();
        // collider properties
        parentCollider.radius = 0.6f;
        // rb properties
        parentRB.bodyType = RigidbodyType2D.Dynamic;
        parentRB.gravityScale = 0f;
        parentRB.mass = rbMass;
        parentRB.linearDamping = rbLinearDamping;
        parentRB.freezeRotation = true;
        // Stop nav mesh
        animal.GetNavMeshAgent().isStopped = true;
        animal.GetNavMeshAgent().ResetPath();
        animal.GetNavMeshAgent().enabled = false;
        // Apply force
        Vector3 direction = animal.transform.position - otherAnimal.transform.position;
        Debug.Log($"=== FINAL RB VALUES ===");
        Debug.Log($"Mass: {parentRB.mass}");
        Debug.Log($"LinearDamping: {parentRB.linearDamping}");
        Debug.Log($"BodyType: {parentRB.bodyType}");
        Debug.Log($"GravityScale: {parentRB.gravityScale}");
        Debug.Log($"Force: {force}");
        Debug.Log($"Velocity BEFORE any AddForce: {parentRB.linearVelocity}");
        parentRB.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        Debug.Log($"Velocity after push: {parentRB.linearVelocity}");
        Debug.Log($"Velocity magnitude: {parentRB.linearVelocity.magnitude}");
    }

    // Reset values post pushing animal
    public void CleanUpAfterPush()
    {
        animal.GetNavMeshAgent().enabled = true;
        Destroy(parentCollider);
        Destroy(parentRB);
    }

    public bool IsTouchingAgent()
    {
        return isTouchingAgent;
    }

    public void IsNotTouchingAgent()
    {
        isTouchingAgent = false;
    }

    public GameObject GetBumpingAnimal()
    {
        return bumpingAnimal;
    }
}
