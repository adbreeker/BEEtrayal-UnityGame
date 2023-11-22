using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBEE : TowerController
{
    public float damage = 10f;
    public float msReduction = 0.2f;

    protected override void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        transform.rotation *= Quaternion.Euler(0, 0, attackSpeed*(360*Time.deltaTime));
    }

    protected override void AttackExecution()
    {
        Collider2D[] insectsInRange = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach(Collider2D insect in insectsInRange)
        {
            InsectController insectController = insect.GetComponent<InsectController>();
            insectController.DealDamage(damage);
            insectController.ReduceMovementSpeed(0.5f, msReduction);
        }
    }
}
