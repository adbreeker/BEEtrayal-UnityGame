using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public struct TowerInfo
{
    public GameObject icon;
    public string name;
    public List<string> stats;
    public List<string> description;
}

public class TowerController : MonoBehaviour
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
    public int price;

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

    public virtual TowerInfo GetTowerInfo()
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
            price.ToString()
        };

        info.description = new List<string>() { towerDescription };

        return info;
    }
}
