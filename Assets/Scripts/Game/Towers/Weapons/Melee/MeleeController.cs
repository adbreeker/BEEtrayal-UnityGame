using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : WeaponController
{
    public void SetUpWeapon(float damage, List<SpecialEffect> specialEffects) 
    {
        _damage = damage;
        _specialEffects = specialEffects;
    }
}
