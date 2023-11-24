using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinoBEE : TowerController
{
    public float damage;
    public float missileSpeed;

    [Header("Missile prefab")]
    public GameObject missilePrefab;

    [Header("Missile spawn point")]
    [SerializeField] Transform[] _missileSpawnPoint = new Transform[4];

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

        GameObject randomInsect = GetRandomInsect();
        if (randomInsect != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, randomInsect.transform.position);
            GameObject missile = Instantiate(missilePrefab, _missileSpawnPoint[Random.Range(0,_missileSpawnPoint.Length)].position, Quaternion.identity);
            missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, randomInsect);
        }
    }

    GameObject GetRandomInsect()
    {
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrder();
        List<GameObject> insectsInRange = new List<GameObject>();
        foreach (InsectController insect in insectsOrder)
        {
            if (Vector3.Distance(transform.position, insect.transform.position) <= attackRange)
            {
                insectsInRange.Add(insect.gameObject);
            }
        }
        if(insectsInRange.Count > 0)
        {
            return insectsInRange[Random.Range(0, insectsInRange.Count)];
        }
        return null;
    }
}
