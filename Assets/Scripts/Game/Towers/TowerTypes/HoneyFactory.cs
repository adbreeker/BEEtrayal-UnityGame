using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyFactory : TowerController
{
    [Header("------------------------------------------", order = -1)]
    [Header("Drop values:")]
    public float dropChance;
    public int dropValue;

    [Header("Missile prefab")]
    [SerializeField] GameObject _missilePrefab;
    [SerializeField] GameObject _explosionPrefab;

    static int _instancesCount = 0;

    protected override void Start()
    {
        base.Start();
        if (isUpgradeActive[3]) { StartCoroutine(ExplosiveCollecting()); }
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
            Vector3 honeyDestination = (Vector2)transform.position + (Random.insideUnitCircle * range);
            Debug.Log("jar distance: " + Vector3.Distance(honeyDestination, transform.position));
            GameObject droppedHoney = Instantiate(_missilePrefab, transform.position, Quaternion.identity);
            droppedHoney.GetComponent<MissileController>().SetUpMissile(missileSpeed, 0, honeyDestination, 0f, _attackSpecialEffects);
            droppedHoney.GetComponent<HoneyDropController>().honeyValue = dropValue;
        }
    }

    IEnumerator ExplosiveCollecting()
    {
        while(true)
        {
            yield return new WaitForSeconds(10f);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Weapon"));
            List<GameObject> dropsToCollect = new List<GameObject>();
            foreach(Collider2D drop in colliders)
            {
                if(drop.tag == "HoneyDrop")
                {
                    dropsToCollect.Add(drop.gameObject);
                    Instantiate(_explosionPrefab, drop.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f))).GetComponent<ExplosionEffect>().explosionSize = 3f;

                    Collider2D[] insectsInArea = Physics2D.OverlapCircleAll(drop.transform.position, 3f, LayerMask.GetMask("Insect"));

                    foreach (Collider2D otherCollider in insectsInArea)
                    {
                        InsectController insect = otherCollider.GetComponent<InsectController>();
                        insect.DealDamage(100f);
                        foreach (SpecialEffect specialEffect in _attackSpecialEffects)
                        {
                            specialEffect.ApplyEffect(insect);
                        }
                    }
                }
            }
            
            foreach(GameObject drop in dropsToCollect)
            {
                drop.AddComponent<CollectingEffect>().Initiate(transform.position, 0.3f);
            }
        }
    }

    //Tower upgrades --------------------------------------------------------------------------------------------- Tower Upgrades
    public override string GetUpgradeDescription(int upgradeIndex)
    {
        switch (upgradeIndex)
        {
            case 1:
                return "Increase honey drop value by 10";
            case 2:
                return "Increase speed by 0,2 and range by 3";
            case 3:
                return "Increase honey drop chance to 40%";
            case 4:
                return "Every 10 seconds causes nearby honey drops to explode dealing 100 damage and then collecting them";
        }

        return "";
    }

    protected override void SetUpgrade1(bool status)
    {
        if (status != isUpgradeActive[0])
        {
            if (status)
            {
                dropValue += 10;
            }
            else
            {
                dropValue -= 10;
            }
            isUpgradeActive[0] = status;
        }
    }
    protected override void SetUpgrade2(bool status)
    {
        if (status != isUpgradeActive[1])
        {
            if (status)
            {
                speed += 0.2f;
                range += 3f;
            }
            else
            {
                speed -= 0.2f;
                range -= 3f;
            }
            isUpgradeActive[1] = status;
        }
    }
    protected override void SetUpgrade3(bool status)
    {
        if (status != isUpgradeActive[2])
        {
            if (status)
            {
                dropChance += 0.15f;
            }
            else
            {
                dropChance -= 0.15f;
            }
            isUpgradeActive[2] = status;
        }
    }
    protected override void SetUpgrade4(bool status)
    {
        if (status != isUpgradeActive[3])
        {
            isUpgradeActive[3] = status;
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
            .Replace("{dropChance}", ((dropChance*100).ToString() + "%"))
            .Replace("{dropValue}", dropValue.ToString())
        };
        for (int i = 0; i < 4; i++)
        {
            if (isUpgradeActive[i])
            {
                info.description.Add(GetUpgradeDescription(i + 1));
            }
        }

        return info;
    }
}
