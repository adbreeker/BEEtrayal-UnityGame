using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBEEnger : TowerController
{
    public float damage = 25f;

    [Header("Missile prefab")]
    public GameObject missilePrefab;

    [Header("Missile spawn point")]
    [SerializeField] Transform[] _missileSpawnPoint = new Transform[2];
    int _spawnPointIndex = 0;

    protected override void Start()
    {
        base.Start();
    }

    protected override void AttackExecution()
    {
        GameObject firstInsect = GetFirstInsect();
        if (firstInsect != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, firstInsect.transform.position);
            GameObject missile = Instantiate(missilePrefab, _missileSpawnPoint[_spawnPointIndex].position, Quaternion.identity, transform);
            missile.AddComponent<BulletController>().SetUpMissile(40.0f, damage, firstInsect);
            _spawnPointIndex = (_spawnPointIndex + 1) % 2;
        }
    }

    GameObject GetFirstInsect()
    {
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrder();
        foreach (InsectController insect in insectsOrder)
        {
            if (Vector3.Distance(transform.position, insect.transform.position) <= attackRange)
            {
                return insect.gameObject;
            }
        }
        return null;
    }
}
