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
        // If collided with the same type of pet
        if (collision.collider.TryGetComponent(out BaseAnimal otherAnimal) && otherAnimal.TypeOfPet == animal.TypeOfPet)
            animal.BreedingPartner = otherAnimal;

        // FIX: OPTIMIZE COLLISION WITH OTHER ANIMALS. DO THIS MORE LIKE THIS IF ^
        // If collided with other animal
        if (collision.gameObject.CompareTag("Animal"))
        {
            IsTouchingAgent = true;
            BumpingAnimal = collision.gameObject.GetComponent<BaseAnimal>();
        }
    }
}