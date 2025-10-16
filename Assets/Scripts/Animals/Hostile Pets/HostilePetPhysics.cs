using UnityEngine;

[RequireComponent(typeof(HostilePet))]
public class HostilePetPhysics : PetPhysics
{
    HostilePet pet;

    protected override void Awake()
    {
        base.Awake();
        pet = GetComponent<HostilePet>();
    }

    // Hanldes collision with other Game Objects
    protected override void OnCollisionEnter2D(Collision2D collision)
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
            // If the other animal is not a wolf set the current prey
            else if (otherAnimal.TypeOfPet != pet.TypeOfPet)
            {
                pet.CurrentPrey = (Pet)otherAnimal;
            }
        }
    }
}
