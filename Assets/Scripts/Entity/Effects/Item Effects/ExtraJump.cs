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

}
