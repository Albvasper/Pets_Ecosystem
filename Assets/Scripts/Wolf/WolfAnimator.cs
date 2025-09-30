using UnityEngine;

public class WolfAnimator : AnimalAnimator
{
    Wolf wolf;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        wolf = GetComponent<Wolf>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void Attack()
    {
        animator.Play("Attack");
    }

    public void IDLE()
    {
        animator.Play("IDLE");
    }
}
