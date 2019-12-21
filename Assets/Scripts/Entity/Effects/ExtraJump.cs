using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraJump : Effect
{

    public ExtraJump()
    {
        effectName = "Extra Jump";
        type = EffectType.ExtraJump;
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override void OnDamagedTrigger(Player player)
    {
        base.OnDamagedTrigger(player);
    }

    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);

        player.mNumJumps++;
    }

    public override void OnUnequipTrigger(Player player)
    {
        base.OnUnequipTrigger(player);
        player.mNumJumps--;

    }

    public override void OnHitTrigger(Attack attack, IHurtable entity)
    {
        base.OnHitTrigger(attack, entity);
    }

    public override void OnJumpTrigger(Player player)
    {
        base.OnJumpTrigger(player);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
