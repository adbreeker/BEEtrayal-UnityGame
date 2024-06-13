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
            price.ToString()
        };

        info.description = new List<string>()
        {
            towerDescription
        };

        return info;
    }
}
