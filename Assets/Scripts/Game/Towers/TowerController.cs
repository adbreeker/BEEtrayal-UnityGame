using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TowerInfo
{
    public GameObject icon;
    public string name;
    public int instancesCount;
    public List<string> stats;
    public int price;
    public List<string> description;
}

public abstract class TowerController : MonoBehaviour
{
    [Header("Tower info:")]
    public GameObject infoPanel;
    public GameObject towerImage;
    public string towerName;
    [TextArea] public string towerDescription;

    [Header("Tower statistics:")]
    public float damage;
    public float range;
    public float speed;
    public float missileSpeed;
    protected List<SpecialEffect> _attackSpecialEffects = new List<SpecialEffect>();

    [Header("Tower upgrades:")]
    public bool[] isUpgradeActive = { false, false, false, false };
    public int[] upgradePrices = { 100, 200, 300, 400 };

    [Header("Tower price")]
    [SerializeField] protected int _price;
    public int GetPrice() { return _price; }

    protected bool _canAttack = true;
    private bool _attackCooldownOngoing = false;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        if(_canAttack)
        {
            AttackExecution();
        }
        else
        {
            if(!_attackCooldownOngoing) 
            {
                _attackCooldownOngoing = true;
                StartCoroutine(AttackCooldown());
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1 / speed);
        _canAttack = true;
        _attackCooldownOngoing = false;
    }

    protected bool IsAnyInsectInRange()
    {
        if(GameParams.insectsManager.GetInsectsOrderInRange(transform.position, range).Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual void AttackExecution() { }


    //Tower upgrades ---------------------------------------------------------------------------------------------------------- Tower upgrades
    public void SetTowerUpgrade(int upgradeIndex, bool status)
    {
        switch (upgradeIndex)
        {
            case 1:
                SetUpgrade1(status);
                break;
            case 2:
                SetUpgrade2(status);
                break;
            case 3:
                SetUpgrade3(status);
                break;
            case 4:
                SetUpgrade4(status);
                break;
        }
    }

    public virtual string GetUpgradeDescription(int upgradeIndex) { return ""; }
    protected virtual void SetUpgrade1(bool status) { isUpgradeActive[0] = status; }
    protected virtual void SetUpgrade2(bool status) { isUpgradeActive[1] = status; }
    protected virtual void SetUpgrade3(bool status) { isUpgradeActive[2] = status; }
    protected virtual void SetUpgrade4(bool status) { isUpgradeActive[3] = status; }

    //Tower meta data --------------------------------------------------------------------------------------------------------- Tower meta data

    public abstract int GetInstancesCount();
    public abstract void SetInstancesCount(int setValue);
    public abstract void ChangeInstancesCount(int valueToAdd);
    public abstract int GetCurrentTowerPrice();

    public abstract TowerInfo GetTowerInfo();
}
