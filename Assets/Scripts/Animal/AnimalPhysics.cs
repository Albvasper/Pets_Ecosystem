using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public class AnimalPhysics : MonoBehaviour
{
    Animal animal;
    bool isTouchingAgent;
    GameObject bumpingAnimal;
    const float linearDamping = 2f;

    void Awake()
    {
        animal = GetComponent<Animal>();
        isTouchingAgent = false;
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

    // Push character in a certain direction
    public void PushAnimal(GameObject otherAnimal, float force)
    {
        animal.GetAnimalBehavior().StopWalking();
        animal.GetRigidbody2D().gravityScale = 0;
        animal.GetRigidbody2D().bodyType = RigidbodyType2D.Dynamic;
        animal.GetRigidbody2D().linearDamping = linearDamping;
        Vector3 direction = animal.transform.position - otherAnimal.transform.position;
        animal.GetRigidbody2D().AddForce(direction.normalized * force, ForceMode2D.Impulse); 
    }

    // Reset values post pushing animal
    public void CleanUpAfterPush()
    {
        animal.GetRigidbody2D().linearVelocity = Vector2.zero;
        animal.GetRigidbody2D().angularVelocity = 0f;
        animal.GetRigidbody2D().bodyType = RigidbodyType2D.Kinematic;
        animal.GetNavMeshAgent().Warp(transform.position);
        animal.GetAnimalBehavior().KeepWalking();
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
