using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinoBEE : TowerController
{
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
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrderInRange(transform.position, range);
        if(insectsOrder.Count > 0)
        {
            return insectsOrder[Random.Range(0, insectsOrder.Count)].gameObject;
        }
        return null;
    }


}
