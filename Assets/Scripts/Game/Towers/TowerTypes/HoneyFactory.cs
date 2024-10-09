using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyFactory : TowerController
{
    public float dropChance;
    public int dropValue;

    [Header("Honey prefab")]
    public GameObject missilePrefab;

    static int _instancesCount = 0;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void AttackExecution()
    {
        _canAttack = false;
        if (Random.Range(0.0f, 1.0f) >= dropChance)
        {
            Vector3 honeyDestination = Random.insideUnitCircle * range;
            GameObject droppedHoney = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            droppedHoney.GetComponent<MissileController>().SetUpMissile(missileSpeed, 0, honeyDestination, _attackSpecialEffects);
            droppedHoney.GetComponent<HoneyDropController>().honeyValue = dropValue;
        }
    }

    //Tower upgrades --------------------------------------------------------------------------------------------- Tower Upgrades
    public override string GetUpgradeDescription(int upgradeIndex)
    {
        switch (upgradeIndex)
        {
            case 1:
                return "Increase honey drop value by 10";
            case 2:
                return "Increase speed and range by 3";
            case 3:
                return "Increase honey drop chance to 40%";
            case 4:
                return "Causes nearby honey drops to explode dealing 100 damage and then collecting them every 10 seconds";
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
            "-",
            range.ToString(),
            speed.ToString(),
            missileSpeed.ToString(),
        };

        info.price = GetCurrentTowerPrice();

        info.description = new List<string>()
        {
            towerDescription
            .Replace("{dropChance}", ((dropChance*100).ToString() + "%"))
            .Replace("{dropValue}", dropValue.ToString())
        };

        return info;
    }
}
