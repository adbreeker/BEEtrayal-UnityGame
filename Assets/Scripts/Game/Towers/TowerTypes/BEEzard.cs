using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BEEzard : TowerController
{
    public float damage;
    public float missileSpeed;
    public float armorReduction;

    [Header("Missile prefab")]
    public GameObject missilePrefab;

    [Header("Missile spawn point")]
    [SerializeField] Transform _missileSpawnPoint;

    List<SpecialEffect> _missileSpecialEffects = new List<SpecialEffect>();

    protected override void Start()
    {
        base.Start();

        _missileSpecialEffects.Add(new SpecialEffects.ArmorReduction(armorReduction));
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

        StartCoroutine(DeleyedAttacks());
    }

    IEnumerator DeleyedAttacks()
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject strongestInsect = GetStrongestInsect();
            if (strongestInsect != null)
            {
                transform.rotation = GameParams.LookAt2D(transform.position, strongestInsect.transform.position);
                GameObject missile = Instantiate(missilePrefab, _missileSpawnPoint.position, Quaternion.identity);
                missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, strongestInsect, _missileSpecialEffects);
            }
            yield return new WaitForSeconds(0.1f / attackSpeed);
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

    public override List<string> GetTowerInfo()
    {
        List<string> towerInfos = new List<string>();

        towerInfos.Add(damage.ToString());
        towerInfos.Add(attackRange.ToString());
        towerInfos.Add(attackSpeed.ToString());
        towerInfos.Add(missileSpeed.ToString());
        towerInfos.Add(armorReduction.ToString());
        towerInfos.Add(price.ToString());

        return towerInfos;
    }
}
