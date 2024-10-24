using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    protected float _damage;
    protected List<SpecialEffect> _specialEffects = new List<SpecialEffect>();


    public virtual void OnInsectPierce(InsectController insect)
    {
        insect.DealDamage(_damage);
        foreach (SpecialEffect specialEffect in _specialEffects)
        {
            specialEffect.ApplyEffect(insect);
        }
    }
}
