using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAttack : Ability
{

    public PoisonAttack()
    {
        abilityName = "Poison Attack";
        type = AbilityType.PoisonAttack;
    }

    public override void OnHitTrigger(Attack attack, IHurtable entity)
    {
        base.OnHitTrigger(attack, entity);
        new Poisoned(entity.GetEntity());
        //entity.GetEntity().statusEffects.Add(new Poisoned(entity.GetEntity()));   
    }

}
