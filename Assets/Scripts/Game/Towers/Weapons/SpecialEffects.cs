using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffect
{
    public virtual void ApplyEffect(InsectController target)
    {

    }
}

namespace SpecialEffects
{
    public class Slow : SpecialEffect
    {
        float _slowTime;
        float _slowPercent;

        public Slow(float slowTime, float slowPercent) 
        {
            _slowTime = slowTime;
            _slowPercent = slowPercent;
        }

        public override void ApplyEffect(InsectController target) 
        {
            target.ReduceMovementSpeed(_slowTime, _slowPercent);
        }
    }

    public class ArmorReduction : SpecialEffect
    {
        float _armorReduction;
        float _duration;

        public ArmorReduction(float armorReduction, float duration = float.PositiveInfinity)
        {
            _armorReduction = armorReduction;
            _duration = duration;
        }

        public override void ApplyEffect(InsectController target)
        {
            target.ReduceArmor(_duration, _armorReduction);
        }
    }

    public class Poison : SpecialEffect
    {
        float _damagePerSecond;

        public Poison(float damagePerSecond)
        {
            _damagePerSecond = damagePerSecond;
        }

        public override void ApplyEffect(InsectController target)
        {
            target.PoisonInsect(_damagePerSecond);
        }
    }

    public class Stun : SpecialEffect
    {
        float _duration;

        public Stun(float duration)
        {
            _duration = duration;
        }

        public override void ApplyEffect(InsectController target)
        {
            target.StunInsect(_duration);
        }
    }
}




