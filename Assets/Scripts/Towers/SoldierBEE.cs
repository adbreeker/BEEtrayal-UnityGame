using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBEE : TowerController
{
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
        Collider2D nearestInsect = Physics2D.OverlapCircle(transform.position, attackRange);
        if (nearestInsect != null)
        {
            LookAt2D(nearestInsect.gameObject.transform.position);
            GameObject missile = Instantiate(missilePrefab, _missileSpawnPoint.position, Quaternion.identity, transform);
            missile.AddComponent<MissileController>().SetUpMissile(true, 10.0f, nearestInsect.gameObject);
        }
    }
}
