using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEffect : StatusEffect
{
    float previousMovespeed;

    public override bool OnApplyEffect(Entity effected)
    {
        if (!base.OnApplyEffect(effected))
        {
            return false;
        }

        previousMovespeed = EffectedEntity.mMovingSpeed;
        EffectedEntity.mMovingSpeed = 0;

        return true;
    }

    public override void UpdateEffect()
    {
        //effectedEntity.Body.mSpeed = Vector2.zero;

        base.UpdateEffect();
    }

    public override void OnEffectEnd()
    {
        EffectedEntity.mMovingSpeed = previousMovespeed;
        base.OnEffectEnd();

    }
}