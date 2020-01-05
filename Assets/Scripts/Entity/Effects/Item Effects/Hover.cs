using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Effect
{

    public Hover()
    {
        effectName = "Hover";
        type = EffectType.Hover;
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override void OnDamagedTrigger(Attack attack)
    {
        base.OnDamagedTrigger(attack);
    }

    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);
        player.mCanHover = true;
    }

    public override void OnUnequipTrigger(Player player)
    {
        base.OnUnequipTrigger(player);
        foreach(Effect effect in player.itemEffects)
        {
            if(effect is Hover)
            {
                return;
            }
        }
        player.mCanHover = false;

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
