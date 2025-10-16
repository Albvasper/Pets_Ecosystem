using UnityEngine;

[RequireComponent(typeof(Pet))]
public class PetPhysics : BasePhysics
{
    Pet pet;

    protected override void Awake()
    {
        base.Awake();
        pet = GetComponent<Pet>();
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
            if (otherAnimal.TypeOfPet == pet.TypeOfPet && otherAnimal.Sex != pet.Sex)
            {
                // Take a chance
                if (Random.value < BaseAnimal.BreedingChance)
                {
                    pet.BreedingPartner = otherAnimal;
                }
            }
        }
    }
}
