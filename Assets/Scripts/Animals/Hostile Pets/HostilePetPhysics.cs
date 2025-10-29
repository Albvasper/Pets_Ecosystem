using UnityEngine;

/// <summary>
/// Class for hostile pets that handles collisions, physics and 
/// pathfinfing.
/// </summary>
public class HostilePetPhysics : PetPhysics
{
    private HostilePet HostilePet;

    protected override void Awake()
    {
        base.Awake();
        HostilePet = GetComponent<HostilePet>();
    }

    // Hanldes collision with other Game Objects
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // If the other gameobject is a cave
        if (collision.gameObject.TryGetComponent<Cave>(out var cave))
        {
            cave.EnterCave(HostilePet);
        }
        // If the other gameobject is an animal
        if (collision.gameObject.TryGetComponent<BaseAnimal>(out var otherAnimal))
        {
            IsTouchingAgent = true;
            BumpingAnimal = otherAnimal;
            /* If the other animal is the same species an the opposite 
                sex AND there is space on the ecosystem. */
            if (Pet_Manager.Instance.Pets.Count < Pet_Manager.Instance.maxPets 
                && otherAnimal.TypeOfPet == HostilePet.TypeOfPet && otherAnimal.Sex !=
                HostilePet.Sex)
            {
                // Take a chance
                if (Random.value < BaseAnimal.BreedingChance)
                {
                    HostilePet.BreedingPartner = otherAnimal;
                }
            }
            // If the other animal is not a hostile pet set the current prey
            else if (otherAnimal.TypeOfPet != HostilePet.TypeOfPet)
            {
                HostilePet.CurrentPrey = otherAnimal;
            }
        }
    }
}
