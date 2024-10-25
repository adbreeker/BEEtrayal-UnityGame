using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBEE : TowerController
{
    [Header("------------------------------------------", order = -1)]
    [Header("Missile prefab")]
    [SerializeField] GameObject _missilePrefab;

    [Header("Missile spawn point")]
    [SerializeField] Transform _missileSpawnPoint;

    static int _instancesCount = 0;

    protected override void Start()
    {
        base.Start();

        if(isUpgradeActive[0]) { _attackSpecialEffects.Add(new SpecialEffects.Slow(0.5f, 0.75f)); }
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

        GameObject strongestInsect = GetStrongestInsect();
        if (strongestInsect != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, strongestInsect.transform.position);
            GameObject missile = Instantiate(_missilePrefab, _missileSpawnPoint.position, GameParams.LookAt2D(transform.position, strongestInsect.transform.position));
            
            if(isUpgradeActive[2] && Random.Range(0,100) < 5) { missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage+500f, strongestInsect, _attackSpecialEffects); }
            else { missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, strongestInsect, _attackSpecialEffects); }
        }
    }

    GameObject GetStrongestInsect()
    {
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrderInRange(transform.position, range);
        if(insectsOrder.Count == 0)
        {
            return null;
        }
        else
        {
            InsectController strongestInsect = insectsOrder[0];
            foreach(InsectController insect in insectsOrder)
            {
                if(insect.health > strongestInsect.health)
                {
                    strongestInsect = insect;
                }
                else if(insect.health == strongestInsect.health && insect.armor > strongestInsect.armor)
                {
                    strongestInsect = insect;
                }
            }
            return strongestInsect.gameObject;
        }
    }

    //Tower upgrades --------------------------------------------------------------------------------------------- Tower Upgrades
    public override string GetUpgradeDescription(int upgradeIndex)
    {
        switch (upgradeIndex)
        {
            case 1:
                return "Attacks slow insects by 75% for 0,5s";
            case 2:
                return "Increase speed by 0,25";
            case 3:
                return "Attacks have 5% chance to deal 500 more damage but multiple intances cost penalty is increased to 300%";
            case 4:
                return "Increase speed by 1,75 but decreas damage by 100";
        }

        return "";
    }

    protected override void SetUpgrade1(bool status)
    {
        if (status != isUpgradeActive[0])
        {
            isUpgradeActive[0] = status;
        }
    }
    protected override void SetUpgrade2(bool status)
    {
        if (status != isUpgradeActive[1])
        {
            if (status)
            {
                speed += 0.25f;
            }
            else
            {
                speed -= 0.25f;
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
                speed += 1.75f;
                damage -= 100f;
            }
            else
            {
                speed -= 1.75f;
                damage += 100f;
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
            "inf",
            speed.ToString(),
            missileSpeed.ToString(),
        };

        info.price = GetCurrentTowerPrice();

        info.description = new List<string>() { towerDescription };
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
