using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Wolf))]
public class WolfPhysics : BasePhysics
{
    Wolf wolf;

    protected override void Awake()
    {
        base.Awake();
        wolf = GetComponent<Wolf>();
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
            if (otherAnimal.TypeOfPet == wolf.TypeOfPet && otherAnimal.Sex != wolf.Sex)
            {
                // Take a chance
                if (Random.value < BaseAnimal.BreedingChance)
                {
                    wolf.BreedingPartner = otherAnimal;
                }
            }
            // If the other animal is not a wolf set the current prey
            else if (otherAnimal.TypeOfPet != wolf.TypeOfPet)
            {
                wolf.CurrentPrey = (Animal)otherAnimal;
            }
        }
    }
}
