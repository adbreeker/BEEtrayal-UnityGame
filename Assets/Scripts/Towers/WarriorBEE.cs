using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBEE : TowerController
{
    public float damage;
    public float msReduction;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        transform.rotation *= Quaternion.Euler(0, 0, attackSpeed*(360*Time.deltaTime));
    }

    protected override void AttackExecution()
    {
        if (IsAnyInsectInRange())
        {
            _canAttack = false;
        }
        else
        {
            return;
        }

        Collider2D[] insectsInRange = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach(Collider2D insect in insectsInRange)
        {
            InsectController insectController = insect.GetComponent<InsectController>();
            insectController.DealDamage(damage);
            insectController.ReduceMovementSpeed(0.5f, msReduction);
        }
    }
}
