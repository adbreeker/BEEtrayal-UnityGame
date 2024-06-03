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

        public ArmorReduction(float armorReduction)
        {
            _armorReduction = armorReduction;
        }

        public override void ApplyEffect(InsectController target)
        {
            target.ReduceArmor(_armorReduction);
        }
    }
}




