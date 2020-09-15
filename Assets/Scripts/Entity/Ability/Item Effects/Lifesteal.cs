using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifesteal : Ability
{
    public int procChance = 10;
    public int lifeGainPercent = 100;

    public override void OnHitTrigger(Attack attack, IHurtable entity)
    {
        base.OnHitTrigger(attack, entity);
        if(Random.Range(0, 100) > procChance)
        {
            return;
        }

       ((Player)owner).GainLife(attack.GetDamage()*(lifeGainPercent/100));
        
    }
}
