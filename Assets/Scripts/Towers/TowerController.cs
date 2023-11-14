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

    protected void LookAt2D(Vector3 targetPosition)
    {
        Vector2 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, -angle);

        transform.rotation = targetRotation;
    }
}
