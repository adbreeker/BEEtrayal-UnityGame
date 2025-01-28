using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BEEzard : TowerController
{
    [Header("------------------------------------------", order = -1)]
    [Header("Armor reduction values:")]
    public float armorReduction;

    [Header("Missile prefab")]
    [SerializeField] GameObject _missilePrefab;
    [SerializeField] GameObject _fireBallPrefab;

    [Header("Missile spawn point")]
    [SerializeField] Transform _missileSpawnPoint;

    static int _instancesCount = 0;

    protected override void Start()
    {
        base.Start();

        _attackSpecialEffects.Add(new SpecialEffects.ArmorReduction(armorReduction));
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

        if (isUpgradeActive[2] && Random.Range(0, 100) < 15)
        {
            GameObject strongestInsect = GetStrongestInsect();

            List<SpecialEffect> specialEffects = new List<SpecialEffect>(_attackSpecialEffects);
            specialEffects.AddRange(_attackSpecialEffects);
            specialEffects.AddRange(_attackSpecialEffects);

            if (isUpgradeActive[3])
            {
                SpecialEffect effect = SpecialEffect.GetRandomEffect();
                if (effect != null) { specialEffects.Add(effect); }
            }

            transform.rotation = GameParams.LookAt2D(transform.position, strongestInsect.transform.position);
            SoundManager.soundManager.PlaySound3D(SoundEnum.ATTACK_MAGIC3, transform.position, true);
            GameObject missile = Instantiate(_fireBallPrefab, _missileSpawnPoint.position, Quaternion.identity);
            missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, 3 * damage + 100f, strongestInsect, specialEffects);
            missile.GetComponent<RocketController>().explosionSize = 2f;
        }
        else { StartCoroutine(DeleyedAttacks()); }
    }

    IEnumerator DeleyedAttacks()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject strongestInsect = GetStrongestInsect();
            if (strongestInsect != null)
            {
                List<SpecialEffect> specialEffects = new List<SpecialEffect>(_attackSpecialEffects);
                if (isUpgradeActive[3])
                {
                    SpecialEffect effect = SpecialEffect.GetRandomEffect();
                    if (effect != null) { specialEffects.Add(effect); }
                }
                transform.rotation = GameParams.LookAt2D(transform.position, strongestInsect.transform.position);
                SoundManager.soundManager.PlaySound3D(SoundEnum.ATTACK_MAGIC1, transform.position, true);
                GameObject missile = Instantiate(_missilePrefab, _missileSpawnPoint.position, Quaternion.identity);
                missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, strongestInsect, specialEffects);
            }
            yield return new WaitForSeconds(0.75f/missileSpeed);
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
                return "Increase missile speed by 10";
            case 2:
                return "Increase speed by 3 but decrease damage by 40";
            case 3:
                return "Every magic missile have 15% chance of being powerful fireball";
            case 4:
                return "Attacks apply additional random effects";
        }

        return "";
    }

    protected override void SetUpgrade1(bool status)
    {
        if (status != isUpgradeActive[0])
        {
            if (status)
            {
                missileSpeed += 10f;
            }
            else
            {
                missileSpeed -= 10f;
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
                speed += 3f;
                damage -= 40f;
            }
            else
            {
                speed -= 3f;
                damage += 40f;
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

        info.description = new List<string>()
        {
            towerDescription
            .Replace("{armorReduction}", armorReduction.ToString())
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
