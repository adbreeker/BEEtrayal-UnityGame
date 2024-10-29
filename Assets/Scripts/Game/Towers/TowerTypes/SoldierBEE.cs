using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBEE : TowerController
{
    [Header("------------------------------------------", order = -1)]
    [Header("Missile prefab")]
    [SerializeField] GameObject _missilePrefab;
    [SerializeField] GameObject _flashGrenadePrefab;
    int _attacksCount;

    [Header("Missile spawn point")]
    [SerializeField] Transform _missileSpawnPoint;
    [SerializeField] Transform _grenadeSpawnPoint;

    static int _instancesCount = 0;

    protected override void Start()
    {
        base.Start();
        _attacksCount = 0;
        if(isUpgradeActive[2]) { _attackSpecialEffects.Add(new SpecialEffects.InstaDeath(0.001f)); }
        if(isUpgradeActive[3]) { _grenadeSpawnPoint.gameObject.SetActive(true); }
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

        GameObject firstInsect = GetFirstInsect();
        if (firstInsect != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, firstInsect.transform.position);
            GameObject missile = Instantiate(_missilePrefab, _missileSpawnPoint.position, Quaternion.identity);
            missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, firstInsect, _attackSpecialEffects);

            if (isUpgradeActive[3])
            {
                _attacksCount++;
                if (_attacksCount >= 30)
                {
                    _attacksCount = 0;

                    List<SpecialEffect> specialEffects = new List<SpecialEffect>() { new SpecialEffects.Stun(1) };
                    GameObject grenade = Instantiate(_flashGrenadePrefab, transform.position, Quaternion.identity);
                    grenade.GetComponent<MissileController>().SetUpMissile(missileSpeed/2f, 0, firstInsect.transform.position, 0f, specialEffects);
                    grenade.GetComponent<GrenadeFlashController>().explosionSize = 1.5f;
                }
            }
        }
    }

    GameObject GetFirstInsect()
    {
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrderInRange(transform.position, range);
        if(insectsOrder.Count > 0)
        {
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
                return "Decrease multiple instances cost penalty to 0%";
            case 2:
                return "Increase speed by 5 but decrease range by 2";
            case 3:
                return "Every attack have 0,1% to kill insect instantly";
            case 4:
                return "Every 30 attacks throws grenade stunning insects for 1s upon explosion";
        }

        return "";
    }

    protected override void SetUpgrade1(bool status)
    {
        if (status != isUpgradeActive[0])
        {
            if (status)
            {
                _multipleInstancesCostPenalty = 0f;
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
            if (status)
            {
                speed += 5f;
                range -= 2f;
            }
            else
            {
                speed -= 5f;
                range += 2f;
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
