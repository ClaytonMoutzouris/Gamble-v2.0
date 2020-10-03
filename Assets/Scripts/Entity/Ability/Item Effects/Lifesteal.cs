using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifesteal : Ability
{
    public int procChance = 10;
    public int lifeGainPercent = 100;

    public override void OnHitTrigger(AttackObject attackObject, IHurtable entity)
    {
        base.OnHitTrigger(attackObject, entity);
        if(Random.Range(0, 100) > procChance)
        {
            return;
        }

       ((Player)owner).GainLife(attackObject.attack.GetDamage()*(lifeGainPercent/100));
        
    }
}
