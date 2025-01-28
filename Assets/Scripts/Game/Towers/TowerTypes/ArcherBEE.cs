using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBEE : TowerController
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

        if(isUpgradeActive[3]) { _attackSpecialEffects.Add(new SpecialEffects.ArmorReduction(100, 0.5f)); }
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
            SoundManager.soundManager.PlaySound3D(SoundEnum.ATTACK_BOW, transform.position, true);
            GameObject missile = Instantiate(_missilePrefab, _missileSpawnPoint.position, GameParams.LookAt2D(transform.position, firstInsect.transform.position) * Quaternion.Euler(0f, 0f, 180f));
            missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, firstInsect.transform.position, range, _attackSpecialEffects);

            if(isUpgradeActive[2])
            {
                Vector3 pos2 = firstInsect.transform.position - transform.position;
                pos2 = Quaternion.Euler(0, 0, 7.5f) * pos2;
                pos2 += transform.position;

                Vector3 pos3 = firstInsect.transform.position - transform.position;
                pos3 = Quaternion.Euler(0, 0, -7.5f) * pos3;
                pos3 += transform.position;

                GameObject missile2 = Instantiate(_missilePrefab, _missileSpawnPoint.position, GameParams.LookAt2D(transform.position, pos2) * Quaternion.Euler(0f, 0f, 180f));
                missile2.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, pos2, range, _attackSpecialEffects);
                GameObject missile3 = Instantiate(_missilePrefab, _missileSpawnPoint.position, GameParams.LookAt2D(transform.position, pos3) * Quaternion.Euler(0f, 0f, 180f));
                missile3.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, pos3, range, _attackSpecialEffects);
            }    
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

    //Tower upgrades --------------------------------------------------------------------------------------------- Tower Upgrades
    public override string GetUpgradeDescription(int upgradeIndex)
    {
        switch (upgradeIndex)
        {
            case 1:
                return "Increase range by 10";
            case 2:
                return "Increase damage by 20";
            case 3:
                return "Shoots 3 arrows at once";
            case 4:
                return "Attacks reduce armor by 100 for 0,5s";
        }

        return "";
    }

    protected override void SetUpgrade1(bool status)
    {
        if (status != isUpgradeActive[0])
        {
            if (status)
            {
                range += 10f;
            }
            else
            {
                range -= 10f;
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
                damage += 20f;
            }
            else
            {
                damage -= 20f;
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
