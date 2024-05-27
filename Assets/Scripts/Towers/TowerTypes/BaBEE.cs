using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaBEE : TowerController
{
    public float damage;
    public float missileSpeed;

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

        GameObject strongestInsect = GetStrongestInsect();
        if (strongestInsect != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, strongestInsect.transform.position);
            GameObject missile = Instantiate(missilePrefab, _missileSpawnPoint.position, GameParams.LookAt2D(transform.position, strongestInsect.transform.position) * Quaternion.Euler(0f, 0f, 180f));
            missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, strongestInsect, _missileSpecialEffects);
        }
    }

    GameObject GetStrongestInsect()
    {
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrderInRange(transform.position, attackRange);
        if (insectsOrder.Count == 0)
        {
            return null;
        }
        else
        {
            InsectController strongestInsect = insectsOrder[0];
            foreach (InsectController insect in insectsOrder)
            {
                if (insect.health > strongestInsect.health)
                {
                    strongestInsect = insect;
                }
                else if (insect.health == strongestInsect.health && insect.armor > strongestInsect.armor)
                {
                    strongestInsect = insect;
                }
            }
            return strongestInsect.gameObject;
        }
    }
}
