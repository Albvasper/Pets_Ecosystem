using UnityEngine;

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
        // If the other gameobject is an animal
        if (collision.gameObject.TryGetComponent<BaseAnimal>(out var otherAnimal))
        {
            IsTouchingAgent = true;
            BumpingAnimal = otherAnimal;
            // If the other animal is the same species an the opposite sex
            if (otherAnimal.TypeOfPet == HostilePet.TypeOfPet && otherAnimal.Sex != HostilePet.Sex)
            {
                // Take a chance
                if (Random.value < BaseAnimal.BreedingChance)
                {
                    HostilePet.BreedingPartner = otherAnimal;
                }
            }
            // If the other animal is not a wolf set the current prey
            else if (otherAnimal.TypeOfPet != HostilePet.TypeOfPet)
            {
                HostilePet.CurrentPrey = (Pet)otherAnimal;
            }
        }
    }
}
