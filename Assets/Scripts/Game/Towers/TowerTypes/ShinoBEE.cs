using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinoBEE : TowerController
{
    [Header("------------------------------------------", order = -1)]
    [Header("Missile prefab")]
    [SerializeField] GameObject _missilePrefab;
    [SerializeField] GameObject _bigShuriken;

    [Header("Missile spawn point")]
    [SerializeField] Transform[] _missileSpawnPoint = new Transform[4];

    static int _instancesCount = 0;

    protected override void Start()
    {
        base.Start();

        if(isUpgradeActive[0]) { _attackSpecialEffects.Add(new SpecialEffects.ArmorReduction(1)); }
        if(isUpgradeActive[1]) { Instantiate(_bigShuriken, transform.parent).GetComponent<MeleeController>().SetUpWeapon(30f, _attackSpecialEffects); }
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

        GameObject randomInsect = GetRandomInsect();
        if (randomInsect != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, randomInsect.transform.position);
            SoundManager.soundManager.PlaySound3D(SoundEnum.ATTACK_THROW_BLADE, transform.position, true);
            GameObject missile = Instantiate(_missilePrefab, _missileSpawnPoint[Random.Range(0,_missileSpawnPoint.Length)].position, Quaternion.identity);
            if(isUpgradeActive[2]) { missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, randomInsect.transform.position, range, _attackSpecialEffects); }
            else { missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, randomInsect, _attackSpecialEffects); }
        }
    }

    GameObject GetRandomInsect()
    {
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrderInRange(transform.position, range);
        if (insectsOrder.Count > 0)
        {
            return insectsOrder[Random.Range(0, insectsOrder.Count)].gameObject;
        }
        return null;
    }

    //Tower upgrades --------------------------------------------------------------------------------------------- Tower Upgrades
    public override string GetUpgradeDescription(int upgradeIndex)
    {
        switch (upgradeIndex)
        {
            case 1:
                return "Attacks decrease struck insects armor by 1";
            case 2:
                return "Big shuriken orbit around dealing 30 damage and applying effects";
            case 3:
                return "Thrown shurikens pierces through insects";
            case 4:
                return "Increase speed by 10 but deacrease damage by 10";
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
                speed += 10f;
                damage -= 10f;
            }
            else
            {
                speed -= 10f;
                damage += 10f;
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
