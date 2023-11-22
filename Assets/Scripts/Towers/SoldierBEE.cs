using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBEE : TowerController
{
    public float damage = 25f;

    [Header("Missile prefab")]
    public GameObject missilePrefab;

    [Header("Missile spawn point")]
    [SerializeField] Transform _missileSpawnPoint;

    protected override void Start()
    {
        base.Start();
    }

    protected override void AttackExecution()
    {
        GameObject firstInsect = GameParams.insectsManager.FirstInsect(gameObject.transform.position, attackRange);
        if (firstInsect != null)
        {
            LookAt2D(firstInsect.transform.position);
            GameObject missile = Instantiate(missilePrefab, _missileSpawnPoint.position, Quaternion.identity, transform);
            missile.AddComponent<BulletController>().SetUpMissile(40.0f, damage, firstInsect);
        }
    }
}
