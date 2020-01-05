using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushProtection : Effect
{
    public CrushProtection()
    {
        effectName = "Crush Protection";
        type = EffectType.CrushProtection;
    }

    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);
        player.crushProtection = true;

    }

    public override void OnUnequipTrigger(Player player)
    {
        base.OnUnequipTrigger(player);
        player.crushProtection = false;

    }
}
