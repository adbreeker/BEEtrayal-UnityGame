using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffect
{
    public virtual void ApplyEffect(InsectController target)
    {

    }

    public static SpecialEffect GetRandomEffect()
    {
        int randomIndex = Random.Range(0, 20) % 10;

        switch(randomIndex)
        {
            case 1:
                return new SpecialEffects.Slow(Random.Range(0.1f, 2f), Random.Range(0.1f, 0.9f));
            case 2:
                return new SpecialEffects.Stun(Random.Range(0.1f, 1.5f));
            case 3:
                return new SpecialEffects.ArmorReduction(Random.Range(0.1f, 10f));
            case 4:
                return new SpecialEffects.ArmorReduction(Random.Range(1f, 25f), Random.Range(0.1f, 1.5f));
            case 5:
                return new SpecialEffects.Poison(Random.Range(0.1f, 3f));
        }

        return null;
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




