using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BEETank : TowerController
{
    [Header("------------------------------------------", order = -1)]
    [Header("Explosion values:")]
    public float explosionSize;

    [Header("Missile prefab")]
    [SerializeField] GameObject _missilePrefab;

    [Header("Missile spawn point")]
    [SerializeField] Transform _missileSpawnPoint;

    static int _instancesCount = 0;

    protected override void Start()
    {
        base.Start();

        if(isUpgradeActive[3]) { _attackSpecialEffects.Add(new SpecialEffects.Poison(5)); }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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
        if(isUpgradeActive[0]) { targetInsect = GetLastInsect(); }
        else { targetInsect = GetFirstInsect(); }

        if (targetInsect != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, targetInsect.transform.position);
            SoundManager.soundManager.PlaySound3D(SoundEnum.ATTACK_CANNON, transform.position, true);
            GameObject missile = Instantiate(_missilePrefab, _missileSpawnPoint.position, GameParams.LookAt2D(transform.position, targetInsect.transform.position));
            missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, targetInsect.transform.position, 0f, _attackSpecialEffects);
            missile.GetComponent<RocketController>().explosionSize = explosionSize;
            
            if (isUpgradeActive[2]) { damage += 0.25f; }
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
    
    GameObject GetLastInsect()
    {
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrderInRange(transform.position, range);
        if (insectsOrder.Count > 0)
        {
            return insectsOrder[insectsOrder.Count-1].gameObject;
        }
        return null;
    }

    //Tower upgrades --------------------------------------------------------------------------------------------- Tower Upgrades
    public override string GetUpgradeDescription(int upgradeIndex)
    {
        switch (upgradeIndex)
        {
            case 1:
                return "Increase range by 2 and target the farthest insects";
            case 2:
                return "Increase speed by 1";
            case 3:
                return "Decrease damage by 30 but damage is increased by 0.25 every shoot";
            case 4:
                return "Decrease damage by 40 but attacks poison insects dealing 5 damage per second";
        }

        return "";
    }

    protected override void SetUpgrade1(bool status)
    {
        if (status != isUpgradeActive[0])
        {
            if (status)
            {
                range += 2f;
            }
            else
            {
                range -= 2f;
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
                speed += 1f;
            }
            else
            {
                speed -= 1f;
            }
            isUpgradeActive[1] = status;
        }
    }
    protected override void SetUpgrade3(bool status)
    {
        if (status != isUpgradeActive[2])
        {
            if (status)
            {
                damage -= 30f;
            }
            else
            {
                damage += 30f;
            }
            isUpgradeActive[2] = status;
        }
    }
    protected override void SetUpgrade4(bool status)
    {
        if (status != isUpgradeActive[3])
        {
            if (status)
            {
                damage -= 40f;
            }
            else
            {
                damage += 40f;
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
            .Replace("{explosionSize}", explosionSize.ToString())
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
