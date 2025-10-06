using UnityEngine;

[RequireComponent(typeof(Animal))]
public class AnimalPhysics : BasePhysics
{
    Animal animal;

    protected override void Awake()
    {
        base.Awake();
        animal = GetComponent<Animal>();
    }

    // Hanldes collision with other Game Objects
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Animal")
        {
            IsTouchingAgent = true;
            BumpingAnimal = collision.gameObject;
        }
    }
}
