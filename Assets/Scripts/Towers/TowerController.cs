using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TowerController : MonoBehaviour
{
    [Header("Tower statistics:")]
    public float attackSpeed;
    public float attackRange;

    protected bool _canAttack = true;
    private bool _attackCooldownOngoing = false;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        if(_canAttack)
        {
            AttackExecution();
        }
        else
        {
            if(!_attackCooldownOngoing) 
            {
                _attackCooldownOngoing = true;
                StartCoroutine(AttackCooldown());
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1 / attackSpeed);
        _canAttack = true;
        _attackCooldownOngoing = false;
    }

    protected bool IsAnyInsectInRange()
    {
        if(GameParams.insectsManager.GetInsectsOrderInRange(transform.position, attackRange).Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual void AttackExecution() { }
}
