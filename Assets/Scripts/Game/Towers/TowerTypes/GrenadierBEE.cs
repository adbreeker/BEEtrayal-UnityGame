using SpecialEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierBEE : TowerController
{
    [Header("------------------------------------------", order = -1)]
    [Header("Explosion values:")]
    public float explosionSize;

    [Header("Missile prefab")]
    [SerializeField] GameObject _missilePrefab;
    [SerializeField] GameObject _stickyGrenadePrefab;

    [Header("Missile spawn point")]
    [SerializeField] Transform[] _missileSpawnPoint = new Transform[3];

    static int _instancesCount = 0;

    protected override void Start()
    {
        base.Start();
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

        StartCoroutine(ThrowThreeGranades());
    }

    IEnumerator ThrowThreeGranades()
    {
        if(isUpgradeActive[2])
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject randomInsect = GetRandomInsect();
                if (randomInsect != null)
                {
                    transform.rotation = GameParams.LookAt2D(transform.position, randomInsect.transform.position);
                    SoundManager.soundManager.PlaySound3D(SoundEnum.ATTACK_THROW_GRENADE, transform.position, true);
                    GameObject missile = Instantiate(_stickyGrenadePrefab, _missileSpawnPoint[i].position, GameParams.LookAt2D(transform.position, randomInsect.transform.position));
                    missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, randomInsect, _attackSpecialEffects);
                    missile.GetComponent<GrenadeController>().explosionSize = explosionSize;
                }
                yield return new WaitForSeconds(0.1f / speed);
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject randomInsect = GetRandomInsect();
                if (randomInsect != null)
                {
                    transform.rotation = GameParams.LookAt2D(transform.position, randomInsect.transform.position);
                    SoundManager.soundManager.PlaySound3D(SoundEnum.ATTACK_THROW_GRENADE, transform.position, true);
                    GameObject missile = Instantiate(_missilePrefab, _missileSpawnPoint[i].position, GameParams.LookAt2D(transform.position, randomInsect.transform.position));
                    missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, randomInsect.transform.position, 0f, _attackSpecialEffects);
                    missile.GetComponent<GrenadeController>().explosionSize = explosionSize;
                }
                yield return new WaitForSeconds(0.1f / speed);
            }
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
                return "Increase missile speed by 30";
            case 2:
                return "Increase explosion size by 1";
            case 3:
                return "Thrown grenades sticks to insects";
            case 4:
                return "Increase damage by 150";
        }

        return "";
    }

    protected override void SetUpgrade1(bool status)
    {
        if (status != isUpgradeActive[0])
        {
            if (status)
            {
                missileSpeed += 30f;
            }
            else
            {
                missileSpeed -= 30f;
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
                explosionSize += 1f;
            }
            else
            {
                explosionSize -= 1f;
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
                damage += 150f;
            }
            else
            {
                damage -= 150f;
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
