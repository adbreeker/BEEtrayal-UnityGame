using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyFactory : TowerController
{
    public float dropChance;
    public int dropValue;

    [Header("Honey prefab")]
    public GameObject missilePrefab;

    static int _instancesCount = 0;

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

    //Tower meta data --------------------------------------------------------------------------------------------------------- Tower meta data

    public override int GetInstancesCount()
    {
        return _instancesCount;
    }

    public override void SetInstancesCount(int setValue)
    {
        _instancesCount = setValue;
    }

    public override void ChangeInstancesCount(int valueToAdd)
    {
        _instancesCount += valueToAdd;
    }

    public override int GetCurrentTowerPrice()
    {
        int currentPrice = _price;
        for (int i = 0; i < _instancesCount; i++)
        {
            currentPrice += (int)(currentPrice * 0.5f);
        }
        return currentPrice;
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
        };

        info.price = GetCurrentTowerPrice();

        info.description = new List<string>()
        {
            towerDescription
            .Replace("{dropChance}", dropChance.ToString())
            .Replace("{dropValue}", dropValue.ToString())
        };

        return info;
    }
}
