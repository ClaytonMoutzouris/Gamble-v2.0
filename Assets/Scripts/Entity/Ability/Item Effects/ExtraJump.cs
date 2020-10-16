using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraJump : Ability
{
    public int numExtraJumps = 1;

    public override void OnGainTrigger(Entity entity)
    {
        base.OnGainTrigger(entity);
        if(entity is Player player)
        {
            player.mNumJumps += numExtraJumps;
        }
    }

    public override void OnRemoveTrigger(Entity entity)
    {
        base.OnRemoveTrigger(entity);
        if(entity is Player player)
        {
            player.mNumJumps -= numExtraJumps;

        }

    }

}
