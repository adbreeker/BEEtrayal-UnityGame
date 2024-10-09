using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    protected float _damage;
    protected List<SpecialEffect> _specialEffects = new List<SpecialEffect>();

    protected bool _isReady = false;

    public void SetUpWeapon(float damage, List<SpecialEffect> specialEffects) 
    {
        _damage = damage;
        _specialEffects = specialEffects;
    }

    public virtual void OnInsectPierce(InsectController insect)
    {
        insect.DealDamage(_damage);
        foreach (SpecialEffect specialEffect in _specialEffects)
        {
            specialEffect.ApplyEffect(insect);
        }
    }
}
