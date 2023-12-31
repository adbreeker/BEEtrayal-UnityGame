using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBEE_Wan : TowerController
{
    public float damage;

    [Header("Weapon")]
    [SerializeField] GameObject _weapon;

    bool _isJumpAttacking = false;

    protected override void Start()
    {
        base.Start();

        _weapon.GetComponent<MeleeController>().SetUpWeapon(damage);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void AttackExecution()
    {
        if (IsAnyInsectInRange() && !_isJumpAttacking)
        {
            _canAttack = false;
            _isJumpAttacking = true;
        }
        else
        {
            return;
        }

        StartCoroutine(JumpAttack());
    }

    IEnumerator JumpAttack()
    {
        GameObject closestInsect = GetClosestInsect();
        if(closestInsect != null) 
        {
            transform.rotation = GameParams.LookAt2D(transform.position, closestInsect.transform.position);

            Vector3 jumpPos = closestInsect.transform.position;
            while(transform.position != jumpPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, jumpPos, 0.4f * attackSpeed);
                yield return new WaitForFixedUpdate();
            }

            float rotationStep = 40 * attackSpeed;
            float currentRotationAngle = 0f;
            float targetRotationAngle = 360f;
            float startingRotationAngle = transform.rotation.eulerAngles.z;
            while (currentRotationAngle < targetRotationAngle)
            {
                currentRotationAngle += rotationStep;
                currentRotationAngle = Mathf.Clamp(currentRotationAngle, 0f, targetRotationAngle);
                Quaternion targetRotation = Quaternion.Euler(0, 0,startingRotationAngle - currentRotationAngle);
                transform.rotation = targetRotation;
                yield return new WaitForFixedUpdate();
            }

            while(transform.localPosition != Vector3.zero)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, 0.4f * attackSpeed);
                yield return new WaitForFixedUpdate();
            }

            
        }
        _isJumpAttacking = false;
    }

    GameObject GetClosestInsect()
    {
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrderInRange(transform.position, attackRange);
        GameObject closestInsect = null;
        foreach (InsectController insect in insectsOrder)
        {
            if (closestInsect == null)
            {
                closestInsect = insect.gameObject;
            }

            if (Vector3.Distance(transform.position, insect.transform.position) < Vector3.Distance(transform.position, closestInsect.transform.position))
            {
                closestInsect = insect.gameObject;
            }
        }
        return closestInsect;
    }
}
