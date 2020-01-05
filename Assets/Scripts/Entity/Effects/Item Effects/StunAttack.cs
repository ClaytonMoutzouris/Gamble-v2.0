using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunAttack : Effect
{

    public StunAttack()
    {
        effectName = "Stun Attack";
        type = EffectType.StunAttack;
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
        new Stunned(entity.GetEntity());

    }

    public override void OnJumpTrigger(Player player)
    {
        base.OnJumpTrigger(player);
    }
}
