using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyFactory : TowerController
{
    public float dropChance;
    public int dropValue;
    public float missileSpeed;

    [Header("Honey prefab")]
    public GameObject missilePrefab;

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
        _canAttack = false;
        if (Random.Range(0.0f, 1.0f) >= dropChance)
        {
            Vector3 honeyDestination = Random.insideUnitCircle * attackRange;
            GameObject droppedHoney = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            droppedHoney.GetComponent<MissileController>().SetUpMissile(missileSpeed, 0, honeyDestination);
            droppedHoney.GetComponent<HoneyDropController>().honeyValue = dropValue;
        }
    }

    public override List<string> GetTowerInfo()
    {
        List<string> towerInfos = new List<string>();

        towerInfos.Add(attackSpeed.ToString());
        towerInfos.Add(attackRange.ToString());
        towerInfos.Add(dropChance.ToString());
        towerInfos.Add(dropValue.ToString());
        towerInfos.Add(missileSpeed.ToString());
        towerInfos.Add(price.ToString());

        return towerInfos;
    }
}
