using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBEE : TowerController
{
    [Header("Missile prefab")]
    public GameObject missilePrefab;

    [Header("Missile spawn point")]
    [SerializeField] Transform _missileSpawnPoint;

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

        GameObject strongestInsect = GetStrongestInsect();
        if (strongestInsect != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, strongestInsect.transform.position);
            GameObject missile = Instantiate(missilePrefab, _missileSpawnPoint.position, GameParams.LookAt2D(transform.position, strongestInsect.transform.position));
            missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, strongestInsect);
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
            price.ToString()
        };

        info.description = new List<string>()
        {
            towerDescription
        };

        return info;
    }
}
