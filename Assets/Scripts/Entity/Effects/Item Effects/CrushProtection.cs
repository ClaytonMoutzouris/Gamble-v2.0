using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushProtection : Ability
{
    public CrushProtection()
    {
        abilityName = "Crush Protection";
        type = AbilityType.CrushProtection;
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
