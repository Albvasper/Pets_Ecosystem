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
        if (collision.gameObject.tag == "Animal")
        {
            IsTouchingAgent = true;
            BumpingAnimal = collision.gameObject;
            if (wolf.CurrentPrey == null)
                wolf.CurrentPrey = collision.gameObject.GetComponent<Animal>();
        }
    }
}
