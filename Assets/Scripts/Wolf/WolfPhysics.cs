using UnityEngine;

public class WolfPhysics : AnimalPhysics
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Animal")
        {
            IsTouchingAgent = true;
            BumpingAnimal = collision.gameObject;
        }
    }

    protected override void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Animal")
        {
            IsTouchingAgent = false;
        }

    }
}
