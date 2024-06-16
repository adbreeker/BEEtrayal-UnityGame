using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBEE : TowerController
{
    public float slowTime;
    public float slowStrenght;

    [Header("Weapon")]
    [SerializeField] GameObject _weapon;

    List<SpecialEffect> _weaponSpecialEffects = new List<SpecialEffect>();

    static int _instancesCount = 0;

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
        transform.rotation *= Quaternion.Euler(0, 0, speed*(360*Time.deltaTime));
    }

    protected override void AttackExecution()
    {
        
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
            "-",
        };
        
        info.price = GetCurrentTowerPrice();

        info.description = new List<string>()
        {
            towerDescription
        };

        return info;
    }
}
