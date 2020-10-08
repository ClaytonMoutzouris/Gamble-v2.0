using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Not currently implemented
public class StatChangeEffect : StatusEffect
{
    public List<StatBonus> statBonuses;

    public override bool OnApplyEffect(Entity effected)
    {
        if (!base.OnApplyEffect(effected))
        {
            return false;
        }

        return true;
    }

    public override void UpdateEffect()
    {
        //effectedEntity.Body.mSpeed = Vector2.zero;

        base.UpdateEffect();
    }

    public override void OnEffectEnd()
    {

        base.OnEffectEnd();

    }
}