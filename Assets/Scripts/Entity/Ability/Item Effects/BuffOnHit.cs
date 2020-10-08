using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffOnHit : Ability
{
    public StatusEffect statusEffect;

    public int duration;
    public int procChance = 5;

    public override void OnHitTrigger(AttackObject attackObject, IHurtable entity)
    {
        base.OnHitTrigger(attackObject, entity);

        int random = Random.Range(0, 100);
        if (random <= procChance)
        {
            StatusEffect effect = ScriptableObject.Instantiate(statusEffect);
            effect.OnApplyEffect(owner);
        }

    }
}
