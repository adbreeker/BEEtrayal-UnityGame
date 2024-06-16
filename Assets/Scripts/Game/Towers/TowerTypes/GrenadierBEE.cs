using SpecialEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierBEE : TowerController
{
    public float explosionSize;

    [Header("Missile prefab")]
    public GameObject missilePrefab;

    [Header("Missile spawn point")]
    [SerializeField] Transform[] _missileSpawnPoint = new Transform[3];

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
        if (IsAnyInsectInRange())
        {
            _canAttack = false;
        }
        else
        {
            return;
        }

        StartCoroutine(ThrowThreeGranades());
    }

    IEnumerator ThrowThreeGranades()
    {
        for(int i = 0; i<3; i++)
        {
            GameObject randomInsect = GetRandomInsect();
            if (randomInsect != null)
            {
                transform.rotation = GameParams.LookAt2D(transform.position, randomInsect.transform.position);
                GameObject missile = Instantiate(missilePrefab, _missileSpawnPoint[i].position, Quaternion.identity);
                missile.GetComponent<MissileController>().SetUpMissile(missileSpeed, damage, randomInsect.transform.position);
                missile.GetComponent<GrenadeController>().explosionSize = explosionSize;
            }
            yield return new WaitForSeconds(0.1f / speed);
        }
    }

    GameObject GetRandomInsect()
    {
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrderInRange(transform.position, range);
        if (insectsOrder.Count > 0)
        {
            return insectsOrder[Random.Range(0, insectsOrder.Count)].gameObject;
        }
        return null;
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
            damage.ToString(),
            range.ToString(),
            speed.ToString(),
            missileSpeed.ToString(),
        };

        info.price = GetCurrentTowerPrice();

        info.description = new List<string>()
        {
            towerDescription
            .Replace("{explosionSize}", explosionSize.ToString())
        };

        return info;
    }
}
