using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraJump : Ability
{

    public ExtraJump()
    {
        abilityName = "Extra Jump";
        type = AbilityType.ExtraJump;
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
