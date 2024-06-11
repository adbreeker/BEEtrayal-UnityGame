using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBEE : TowerController
{
    public float damage;
    public float slowTime;
    public float slowStrenght;

    [Header("Weapon")]
    [SerializeField] GameObject _weapon;

    List<SpecialEffect> _weaponSpecialEffects = new List<SpecialEffect>();

    protected override void Start()
    {
        base.Start();

        _weaponSpecialEffects.Add(new SpecialEffects.Slow(slowTime, slowStrenght));

        _weapon.GetComponent<MeleeController>().SetUpWeapon(damage, _weaponSpecialEffects);
    }

    protected override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        transform.rotation *= Quaternion.Euler(0, 0, attackSpeed*(360*Time.deltaTime));
    }

    protected override void AttackExecution()
    {
        
    }

    public override List<string> GetTowerInfo()
    {
        List<string> towerInfos = new List<string>();

        towerInfos.Add(damage.ToString());
        towerInfos.Add(attackRange.ToString());
        towerInfos.Add(attackSpeed.ToString());
        towerInfos.Add(slowTime.ToString());
        towerInfos.Add(slowStrenght.ToString());
        towerInfos.Add(price.ToString());

        return towerInfos;
    }
}
