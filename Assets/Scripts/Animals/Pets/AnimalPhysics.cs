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
        // If the other gameobject is an animal
        if (collision.gameObject.TryGetComponent<BaseAnimal>(out var otherAnimal))
        {
            IsTouchingAgent = true;
            BumpingAnimal = otherAnimal;
            // If the other animal is the same species an the opposite sex
            if (otherAnimal.TypeOfPet == animal.TypeOfPet && otherAnimal.Sex != animal.Sex)
            {
                // Take a chance
                if (Random.value < BaseAnimal.BreedingChance)
                {
                    animal.BreedingPartner = otherAnimal;
                }
            }
        }
    }
}