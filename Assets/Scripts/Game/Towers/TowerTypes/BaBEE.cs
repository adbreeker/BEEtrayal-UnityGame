using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaBEE : TowerController
{
    [Header("------------------------------------------", order = -1)]
    [Header("Slow values:")]
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
        if(isUpgradeActive[1]) { _attackSpecialEffects.Add(new SpecialEffects.Poison(1)); }
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

        GameObject strongestInsect = GetStrongestInsect();
        if (strongestInsect != null)
        {
            List<SpecialEffect> specialEffects = new List<SpecialEffect>(_attackSpecialEffects);
            if(isUpgradeActive[3] && Random.Range(0,100) < 15)
            {
                specialEffects.Add(new SpecialEffects.Stun(0.75f));
            }

            transform.rotation = GameParams.LookAt2D(transform.position, strongestInsect.transform.position);
            SoundManager.soundManager.PlaySound3D(SoundEnum.ATTACK_BLOWPIPE, transform.position, true);
            GameObject missile = Instantiate(_missilePrefab, _missileSpawnPoint.position, GameParams.LookAt2D(transform.position, strongestInsect.transform.position) * Quaternion.Euler(0f, 0f, 180f));
            missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, strongestInsect, specialEffects);
        }
    }

    GameObject GetStrongestInsect()
    {
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrderInRange(transform.position, range);
        if (insectsOrder.Count == 0)
        {
            return null;
        }
        else
        {
            InsectController strongestInsect = insectsOrder[0];
            foreach (InsectController insect in insectsOrder)
            {
                if (insect.health > strongestInsect.health)
                {
                    strongestInsect = insect;
                }
                else if (insect.health == strongestInsect.health && insect.armor > strongestInsect.armor)
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
                return "First one is free but multiple instances cost penalty is increased to 75%";
            case 2:
                return "Attacks also poison insects dealing 1 damage per second";
            case 3:
                return "Increase speed by 1";
            case 4:
                return "Attacks have 15% chance to stun insect for 0.75s";
        }

        return "";
    }

    protected override void SetUpgrade1(bool status)
    {
        if (status != isUpgradeActive[0])
        {
            if (status)
            {
                _multipleInstancesCostPenalty = 0.75f;
            }
            else
            {
                _multipleInstancesCostPenalty = 0.5f;
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
            if (status)
            {
                speed += 1f;

            }
            else
            {
                speed -= 1f;
            }
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
        if(isUpgradeActive[0] && _instancesCount == 0) { return 0; }

        int currentPrice = _price;
        for (int i = 0; i < GetInstancesCount(); i++)
        {
            currentPrice += (int)(currentPrice * _multipleInstancesCostPenalty);
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
