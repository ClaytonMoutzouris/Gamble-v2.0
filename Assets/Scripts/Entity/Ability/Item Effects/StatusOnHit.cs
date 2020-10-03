using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusOnHit : Ability
{
    public StatusEffectType statusType;
    public int duration;
    public int procChance = 5;

    public override void OnHitTrigger(AttackObject attackObject, IHurtable entity)
    {
        base.OnHitTrigger(attackObject, entity);
        if (entity is BossEnemy || entity is Miniboss)
        {
            return;
        }

        int random = Random.Range(0, 100);
        if (random < procChance)
        {
            StatusEffect effect = StatusEffect.GetEffectFromType(statusType);
            effect.OnApplyEffect(entity.GetEntity(), duration);
        }

    }
}
