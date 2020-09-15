using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusOnHit : Ability
{
    public StatusEffectType statusType;
    public int duration;

    public override void OnHitTrigger(Attack attack, IHurtable entity)
    {
        base.OnHitTrigger(attack, entity);
        if (entity is BossEnemy || entity is Miniboss)
        {
            return;
        }

        StatusEffect effect = StatusEffect.GetEffectFromType(statusType);
        effect.OnApplyEffect(entity.GetEntity(), duration);
        

    }
}
