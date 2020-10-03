using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraJump : Ability
{
    public int numExtraJumps = 1;

    public override void OnEquipTrigger(Entity entity)
    {
        base.OnEquipTrigger(entity);
        if(entity is Player player)
        {
            player.mNumJumps += numExtraJumps;
        }
    }

    public override void OnUnequipTrigger(Entity entity)
    {
        base.OnUnequipTrigger(entity);
        if(entity is Player player)
        {
            player.mNumJumps -= numExtraJumps;

        }

    }

}
