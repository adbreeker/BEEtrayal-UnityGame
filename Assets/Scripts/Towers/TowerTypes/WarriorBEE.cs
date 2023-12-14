using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBEE : TowerController
{
    public float damage;
    public float msReduction;

    [Header("Weapon")]
    [SerializeField] GameObject _weapon;

    List<SpecialEffect> _weaponSpecialEffects = new List<SpecialEffect>();

    protected override void Start()
    {
        base.Start();

        _weaponSpecialEffects.Add(new SpecialEffects.Slow(0.5f, msReduction));

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
}
