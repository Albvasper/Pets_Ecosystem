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

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // If collided with the same type of pet
        if (collision.collider.TryGetComponent(out BaseAnimal otherAnimal) && otherAnimal.TypeOfPet == wolf.TypeOfPet)
            wolf.BreedingPartner = otherAnimal;

        // FIX: OPTIMIZE COLLISION WITH OTHER ANIMALS. DO THIS MORE LIKE THIS IF ^
        // If collided with other animal
        if (collision.gameObject.tag == "Animal")
        {
            IsTouchingAgent = true;
            BumpingAnimal = collision.gameObject.GetComponent<BaseAnimal>();
            // FIX: IF A WOLF COLLIDES WITH A WOLF THERE WILL CAUSE AN ERROR
            if (wolf.CurrentPrey == null)
                wolf.CurrentPrey = collision.gameObject.GetComponent<Animal>();
        }
    }
}
