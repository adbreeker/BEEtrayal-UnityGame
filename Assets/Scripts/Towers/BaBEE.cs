using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MissileController;

public class BaBEE : TowerController
{
    public float damage = 75f;

    [Header("Missile prefab")]
    public GameObject missilePrefab;

    [Header("Missile spawn point")]
    [SerializeField] Transform _missileSpawnPoint;

    List<SpecialEffect> _missileSpecialEffects = new List<SpecialEffect>();

    protected override void Start()
    {
        base.Start();

        _missileSpecialEffects.Add(new SpecialEffects.Slow(2.0f, 0.5f));
    }

    protected override void AttackExecution()
    {
        GameObject strongestInsect = GetStrongestInsect();
        if (strongestInsect != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, strongestInsect.transform.position);
            GameObject missile = Instantiate(missilePrefab, _missileSpawnPoint.position, Quaternion.identity, transform);
            missile.AddComponent<DartController>().SetUpMissile(15.0f, damage, strongestInsect, _missileSpecialEffects);
        }
    }

    GameObject GetStrongestInsect()
    {
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrder();

        InsectController strongestInsect = null;
        foreach (InsectController insect in insectsOrder)
        {
            if (Vector3.Distance(transform.position, insect.transform.position) <= attackRange)
            {
                if(strongestInsect == null)
                {
                    strongestInsect = insect;
                }

                if (insect.health > strongestInsect.health)
                {
                    strongestInsect = insect;
                }
                else if (insect.health == strongestInsect.health && insect.armor > strongestInsect.armor)
                {
                    strongestInsect = insect;
                }
            }
        }

        if(strongestInsect == null)
        {
            return null;
        }
        else
        {
            return strongestInsect.gameObject;
        }
    }
}