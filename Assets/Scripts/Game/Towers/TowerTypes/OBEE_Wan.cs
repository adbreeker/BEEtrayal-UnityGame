using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBEE_Wan : TowerController
{
    [Header("Weapon")]
    [SerializeField] GameObject _weapon;

    bool _isJumpAttacking = false;

    static int _instancesCount = 0;

    protected override void Start()
    {
        base.Start();

        _weapon.GetComponent<MeleeController>().SetUpWeapon(damage, _attackSpecialEffects);
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
                transform.position = Vector3.MoveTowards(transform.position, jumpPos, 0.4f * speed);
                yield return new WaitForFixedUpdate();
            }

            float rotationStep = 40 * speed;
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
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, 0.4f * speed);
                yield return new WaitForFixedUpdate();
            }

            
        }
        _isJumpAttacking = false;
    }

    GameObject GetClosestInsect()
    {
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrderInRange(transform.position, range);
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

    //Tower upgrades --------------------------------------------------------------------------------------------- Tower Upgrades
    public override string GetUpgradeDescription(int upgradeIndex)
    {
        switch (upgradeIndex)
        {
            case 1:
                return "Increase damage by 10";
            case 2:
                return "Increase damage by 25 and range by 1";
            case 3:
                return "Holds 1 more sword";
            case 4:
                return "While jumping throws up to 3 daggers that reduce armor by 50";
        }

        return "";
    }

    protected override void SetUpgrade1(bool status)
    {
        if (status != isUpgradeActive[0])
        {
            if (status)
            {
                range += 1.5f;

            }
            else
            {
                range -= 1.5f;
            }
            isUpgradeActive[0] = status;
        }
    }
    protected override void SetUpgrade2(bool status)
    {
        if (status != isUpgradeActive[1])
        {
            isUpgradeActive[1] = status;
        }
    }
    protected override void SetUpgrade3(bool status)
    {
        if (status != isUpgradeActive[2])
        {
            isUpgradeActive[2] = status;
        }
    }
    protected override void SetUpgrade4(bool status)
    {
        if (status != isUpgradeActive[3])
        {
            isUpgradeActive[3] = status;
        }
    }

    //Tower meta data --------------------------------------------------------------------------------------------------------- Tower meta data

    public override int GetInstancesCount()
    {
        return _instancesCount;
    }

    public override void SetInstancesCount(int setValue)
    {
        _instancesCount = setValue;
    }

    public override void ChangeInstancesCount(int valueToAdd)
    {
        _instancesCount += valueToAdd;
    }

    public override int GetCurrentTowerPrice()
    {
        int currentPrice = _price;
        for (int i = 0; i < _instancesCount; i++)
        {
            currentPrice += (int)(currentPrice * 0.5f);
        }
        return currentPrice;
    }

    public override TowerInfo GetTowerInfo()
    {
        TowerInfo info = new TowerInfo();

        info.icon = towerImage;
        info.name = towerName;

        info.stats = new List<string>()
        {
            damage.ToString(),
            range.ToString(),
            speed.ToString(),
            "-",
        };

        info.price = GetCurrentTowerPrice();

        info.description = new List<string>()
        {
            towerDescription
        };

        return info;
    }
}
