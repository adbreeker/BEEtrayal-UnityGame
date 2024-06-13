using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyFactory : TowerController
{
    public float dropChance;
    public int dropValue;

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
            Vector3 honeyDestination = Random.insideUnitCircle * range;
            GameObject droppedHoney = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            droppedHoney.GetComponent<MissileController>().SetUpMissile(missileSpeed, 0, honeyDestination);
            droppedHoney.GetComponent<HoneyDropController>().honeyValue = dropValue;
        }
    }

    public override TowerInfo GetTowerInfo()
    {
        TowerInfo info = new TowerInfo();

        info.icon = towerImage;
        info.name = towerName;

        info.stats = new List<string>()
        {
            "-",
            range.ToString(),
            speed.ToString(),
            missileSpeed.ToString(),
            price.ToString()
        };

        info.description = new List<string>()
        {
            towerDescription
            .Replace("{dropChance}", dropChance.ToString())
            .Replace("{dropValue}", dropValue.ToString())
        };

        return info;
    }
}
