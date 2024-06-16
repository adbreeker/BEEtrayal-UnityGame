using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BEEzard : TowerController
{
    public float armorReduction;

    [Header("Missile prefab")]
    public GameObject missilePrefab;

    [Header("Missile spawn point")]
    [SerializeField] Transform _missileSpawnPoint;

    List<SpecialEffect> _missileSpecialEffects = new List<SpecialEffect>();

    static int _instancesCount = 0;

    protected override void Start()
    {
        base.Start();

        _missileSpecialEffects.Add(new SpecialEffects.ArmorReduction(armorReduction));
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

        StartCoroutine(DeleyedAttacks());
    }

    IEnumerator DeleyedAttacks()
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject strongestInsect = GetStrongestInsect();
            if (strongestInsect != null)
            {
                transform.rotation = GameParams.LookAt2D(transform.position, strongestInsect.transform.position);
                GameObject missile = Instantiate(missilePrefab, _missileSpawnPoint.position, Quaternion.identity);
                missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, strongestInsect, _missileSpecialEffects);
            }
            yield return new WaitForSeconds(0.1f / speed);
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

        return info;
    }
}
