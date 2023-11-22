using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TowerController : MonoBehaviour
{
    [Header("Tower statistics:")]
    public float attackSpeed = 1;
    public float attackRange = 3;
    protected virtual void Start()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        while (true) 
        {
            AttackExecution();
            yield return new WaitForSeconds(1 / attackSpeed);
        }
    }

    protected virtual void AttackExecution() { }
}
