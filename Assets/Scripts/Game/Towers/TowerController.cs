using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

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

    [Header("Tower price")]
    [SerializeField] protected int _price;

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

    //Tower meta data --------------------------------------------------------------------------------------------------------- Tower meta data

    public abstract int GetInstancesCount();
    public abstract void SetInstancesCount(int setValue);
    public abstract void ChangeInstancesCount(int valueToAdd);
    public abstract int GetCurrentTowerPrice();

    public abstract TowerInfo GetTowerInfo();
}
