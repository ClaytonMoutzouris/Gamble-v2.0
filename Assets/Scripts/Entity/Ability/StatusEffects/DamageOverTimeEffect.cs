using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeEffect : StatusEffect
{

    public float tickFrequency = 1;
    float tickTimer = 0;
    public int tickDamage = 1;
    Attack attack;

    public override bool OnApplyEffect(Entity effected)
    {
        if(!base.OnApplyEffect(effected))
        {
            return false;
        }
        //If the effect isnt stackable and the entity is already effected by the same effect
        //we refresh the timer and do not add a new one
        attack = new Attack(tickDamage);


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
            hurtable.GetHurt(attack);
        }
    }

    public override void OnEffectEnd()
    {
        base.OnEffectEnd();
    }
}
