using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraEffect : Effect
{
    MeleeAttack attack;

    public AuraEffect()
    {
    }

    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);
        //attack = new MeleeAttack()

    }

    public override void OnUnequipTrigger(Player player)
    {
        base.OnUnequipTrigger(player);
    }
}
