using UnityEngine;

public class Wolf : Animal
{
    public bool Hunting { get; set;}
    public WolfAnimator WolfAnimator {get; set;}
    public WolfPhysics WolfPhysics {get; set;}
    public WolfBehavior WolfBehavior {get; set;}

    protected override void Awake()
    {
        base.Awake();
        Hunting = false;
        WolfAnimator = GetComponent<WolfAnimator>();
        WolfPhysics = GetComponent<WolfPhysics>();
        WolfBehavior = GetComponent<WolfBehavior>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        // If an animal is touching another agent => cooldown from changing states
        counter += Time.deltaTime;
        if (AnimalPhysics.IsTouchingAgent)
        {
            if (counter >= BumpingCooldown)
            {
                WolfBehavior.SetState(new State_WolfBumping(this, WolfPhysics.BumpingAnimal));
                counter = 0;
            }
        }

        if (Hunting == true && AnimalPhysics.IsTouchingAgent == true)
        {
            WolfBehavior.SetState(new State_AttackPrey(this, WolfPhysics.BumpingAnimal.GetComponent<Animal>()));
        }
    }
}
