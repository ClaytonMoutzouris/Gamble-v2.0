using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAttack : Effect
{

    public PoisonAttack()
    {
        effectName = "Poison Attack";
        type = EffectType.PoisonAttack;
    }

    public override void OnHitTrigger(Attack attack, IHurtable entity)
    {
        base.OnHitTrigger(attack, entity);
        new Poisoned(entity.GetEntity());
        //entity.GetEntity().statusEffects.Add(new Poisoned(entity.GetEntity()));   
    }

}
