using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : Effect
{
    MeleeAttack attack;

    public Aura()
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
