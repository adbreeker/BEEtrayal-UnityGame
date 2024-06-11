using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BEETank : TowerController
{
    public float damage;
    public float missileSpeed;
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
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrderInRange(transform.position, attackRange);
        if (insectsOrder.Count > 0)
        {
            return insectsOrder[0].gameObject;
        }
        return null;
    }

    public override List<string> GetTowerInfo()
    {
        List<string> towerInfos = new List<string>();

        towerInfos.Add(damage.ToString());
        towerInfos.Add(attackRange.ToString());
        towerInfos.Add(attackSpeed.ToString());
        towerInfos.Add(missileSpeed.ToString());
        towerInfos.Add(explosionSize.ToString());
        towerInfos.Add(price.ToString());

        return towerInfos;
    }
}
