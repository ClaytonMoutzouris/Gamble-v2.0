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

    public override void OnDamagedTrigger(Player player)
    {
        base.OnDamagedTrigger(player);
    }

    public override void OnEquipTrigger(Player player)
    {
        player.mCanHover = true;
    }

    public override void OnUnequipTrigger(Player player)
    {
        player.mCanHover = false;

    }

    public override void OnHitTrigger(Player player)
    {
        base.OnHitTrigger(player);
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
