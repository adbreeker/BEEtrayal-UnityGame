using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBEE : TowerController
{
    public float damage = 300f;

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
        GameObject strongestInsect = GetStrongestInsect();
        if (strongestInsect != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, strongestInsect.transform.position);
            GameObject missile = Instantiate(missilePrefab, _missileSpawnPoint.position, Quaternion.identity, transform);
            missile.AddComponent<BoltController>().SetUpMissile(75.0f, damage, strongestInsect);
        }
    }

    GameObject GetStrongestInsect()
    {
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrder();
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
}
