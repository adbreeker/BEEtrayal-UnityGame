using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBEEnger : TowerController
{
    [Header("------------------------------------------", order = -1)]
    [Header("Missile prefab")]
    [SerializeField] GameObject _missilePrefab;

    [Header("Missile spawn point")]
    [SerializeField] Transform[] _missileSpawnPoint = new Transform[2];
    int _spawnPointIndex = 0;

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
            //SoundManager.soundManager.PlaySound(SoundEnum.ATTACK_PISTOL);
            GameObject missile = Instantiate(_missilePrefab, _missileSpawnPoint[_spawnPointIndex].position, Quaternion.identity);
            missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, firstInsect, _attackSpecialEffects);
            _spawnPointIndex = (_spawnPointIndex + 1) % 2;
        }

        if(isUpgradeActive[2])
        {
            firstInsect = GetSecondInsect();
            if (firstInsect != null)
            {
                transform.rotation = GameParams.LookAt2D(transform.position, firstInsect.transform.position);
                SoundManager.soundManager.PlaySound(SoundEnum.ATTACK_PISTOL);
                GameObject missile = Instantiate(_missilePrefab, _missileSpawnPoint[_spawnPointIndex].position, Quaternion.identity);
                missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, firstInsect, _attackSpecialEffects);
                _spawnPointIndex = (_spawnPointIndex + 1) % 2;
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

    GameObject GetSecondInsect()
    {
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrderInRange(transform.position, range);
        if (insectsOrder.Count > 1)
        {
            return insectsOrder[1].gameObject;
        }
        return null;
    }

    //Tower upgrades --------------------------------------------------------------------------------------------- Tower Upgrades
    public override string GetUpgradeDescription(int upgradeIndex)
    {
        switch (upgradeIndex)
        {
            case 1:
                return "Increase range by 1.5";
            case 2:
                return "Multiple instances cost penalty is decreased to 25%";
            case 3:
                return "Shoot two insects at once";
            case 4:
                return "Decrease speed by 1 and increase damage by 100";
        }

        return "";
    }

    protected override void SetUpgrade1(bool status)
    {
        if(status != isUpgradeActive[0])
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
            if (status)
            {
                _multipleInstancesCostPenalty = 0.25f;
            }
            else
            {
                _multipleInstancesCostPenalty = 0.5f;
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
                speed -= 1.5f;
                damage += 100;

            }
            else
            {
                speed += 1.5f;
                damage -= 100;
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
        for(int i=0; i<4; i++)
        {
            if(isUpgradeActive[i]) 
            {
                info.description.Add(GetUpgradeDescription(i + 1));
            }
        }

        return info;
    }

}
