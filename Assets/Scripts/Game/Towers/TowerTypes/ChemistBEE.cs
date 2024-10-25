using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemistBEE : TowerController
{
    public float slowTime;
    public float slowStrength;

    [Header("Missile prefab")]
    [SerializeField] GameObject _missilePrefab;

    [Header("Missile spawn point")]
    [SerializeField] Transform _missileSpawnPoint;

    static int _instancesCount = 0;

    protected override void Start()
    {
        base.Start();

        _attackSpecialEffects.Add(new SpecialEffects.Slow(slowTime, slowStrength));
    }

    protected override void Update()
    {
        base.Update();
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

        GameObject targetInsect;
        if(isUpgradeActive[2]) { targetInsect = GetNotSlowedInsect(); }
        else { targetInsect = GetFirstInsect(); }

        if (targetInsect != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, targetInsect.transform.position);
            GameObject missile = Instantiate(_missilePrefab, _missileSpawnPoint.position, GameParams.LookAt2D(transform.position, targetInsect.transform.position) * Quaternion.Euler(0f, 0f, 180f));
            missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, targetInsect, _attackSpecialEffects);
            
            if(isUpgradeActive[3]) { missile.GetComponent<BlobMissileController>().SetUpHoneyBlob(5, 1f); }
        }
    }

    GameObject GetFirstInsect()
    {
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrderInRange(transform.position, range);
        if (insectsOrder.Count > 0)
        {
            return insectsOrder[0].gameObject;
        }
        return null;
    }

    GameObject GetNotSlowedInsect()
    {
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrderInRange(transform.position, range);
        if (insectsOrder.Count > 0)
        {
            foreach(InsectController insect in insectsOrder)
            {
                if(!insect.isSlowed) { return insect.gameObject; }
            }

            return insectsOrder[0].gameObject;
        }
        return null;
    }

    //Tower upgrades --------------------------------------------------------------------------------------------- Tower Upgrades
    public override string GetUpgradeDescription(int upgradeIndex)
    {
        switch (upgradeIndex)
        {
            case 1:
                return "Slow strength increased to 85%";
            case 2:
                return "Slow time increased by 1s";
            case 3:
                return "Targeting currently not slowed insects instead of first ones";
            case 4:
                return "Reduce speed by 1 but attacks leaves sticky honey on the ground";
        }

        return "";
    }

    protected override void SetUpgrade1(bool status)
    {
        if (status != isUpgradeActive[0])
        {
            if (status)
            {
                slowStrength += 0.1f;
            }
            else
            {
                slowStrength -= 0.1f;
            }
            isUpgradeActive[0] = status;
        }
    }
    protected override void SetUpgrade2(bool status)
    {
        if (status != isUpgradeActive[1])
        {
            if (status)
            {
                slowTime += 1f;
            }
            else
            {
                slowTime -= 1f;
            }
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
            if (status)
            {
                speed -= 1f;
            }
            else
            {
                speed += 1f;
            }
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
            missileSpeed.ToString(),
        };

        info.price = GetCurrentTowerPrice();

        info.description = new List<string>()
        {
            towerDescription
            .Replace("{slowStrength}", ((slowStrength*100).ToString() + "%"))
            .Replace("{slowTime}", slowTime.ToString())
        };

        for (int i = 0; i < 4; i++)
        {
            if (isUpgradeActive[i])
            {
                info.description.Add(GetUpgradeDescription(i + 1));
            }
        }

        return info;
    }
}
