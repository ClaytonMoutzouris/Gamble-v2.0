using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnHit : Ability
{
    Effect effect;

    public override void OnHitTrigger(Attack attack, IHurtable entity)
    {
        base.OnHitTrigger(attack, entity);
        //effect.ApplyEffect();
    }


}
