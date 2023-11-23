using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBEE : TowerController
{
    public float damage = 75f;

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
        GameObject firstInsect = GetFirstInsect();
        if (firstInsect != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, firstInsect.transform.position);
            GameObject missile = Instantiate(missilePrefab, _missileSpawnPoint.position, Quaternion.identity, transform);
            missile.AddComponent<ArrowController>().SetUpMissile(30.0f, damage, CalculateArrowPoint(firstInsect.transform.position));
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

    Vector3 CalculateArrowPoint(Vector3 insectPos)
    {
        Vector3 direction = (insectPos - transform.position).normalized;
        Vector3 arrowPoint = insectPos + (direction * 10.0f);
        return arrowPoint;
    }
}
