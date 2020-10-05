using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOverTimeEffect : StatusEffect
{

    public float tickFrequency = 1;
    float tickTimer = 0;
    public int tickHeal = 2;

    public override bool OnApplyEffect(Entity effected)
    {
        if (!base.OnApplyEffect(effected))
        {
            return false;
        }

        //effectOffset = effected.Body.mAABB.HalfSizeY * Vector2.up;
        return true;
    }

    public override void UpdateEffect()
    {
        tickTimer += Time.deltaTime;

        if (tickTimer >= tickFrequency)
        {
            OnTickTrigger();
        }
        base.UpdateEffect();
    }

    public void OnTickTrigger()
    {
        tickTimer = 0;

        if (EffectedEntity is IHurtable hurtable)
        {
            hurtable.GainLife(tickHeal, false);
        }
    }

    public override void OnEffectEnd()
    {
        base.OnEffectEnd();
    }
}
