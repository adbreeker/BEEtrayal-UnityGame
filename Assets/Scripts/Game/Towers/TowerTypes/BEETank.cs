using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BEETank : TowerController
{
    public float explosionSize;

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

        GameObject firstInsect = GetFirstInsect();
        if (firstInsect != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, firstInsect.transform.position);
            GameObject missile = Instantiate(missilePrefab, _missileSpawnPoint.position, GameParams.LookAt2D(transform.position, firstInsect.transform.position));
            missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, firstInsect.transform.position);
            missile.GetComponent<RocketController>().explosionSize = explosionSize;
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
            price.ToString()
        };

        info.description = new List<string>()
        {
            towerDescription
            .Replace("{explosionSize}", explosionSize.ToString())
        };

        return info;
    }
}
