using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunAttack : Ability
{

    public StunAttack()
    {
        abilityName = "Stun Attack";
        type = AbilityType.StunAttack;
    }

    public override void OnDamagedTrigger(Attack attack)
    {
        base.OnDamagedTrigger(attack);
    }

    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);
    }

    public override void OnUnequipTrigger(Player player)
    {
        base.OnUnequipTrigger(player);

    }

    public override void OnHitTrigger(Attack attack, IHurtable entity)
    {
        base.OnHitTrigger(attack, entity);
        if(entity is BossEnemy || entity is Miniboss)
        {
            return;
        }
        new Stunned(entity.GetEntity());

    }

}
