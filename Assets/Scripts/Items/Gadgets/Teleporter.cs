using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : Gadget
{

    //Range of the teleporter in tiles
    public int range;

    public override bool Activate(Player player, int index)
    {
        Debug.Log("Attempting to active teleporter");
        if (!base.Activate(player, index))
        {
            return false;
        }

        Vector2 aim;

        if (index == 1)
        {
            aim = player.GetAimLeft();
        }
        else
        {
            aim = player.GetAimRight();
        }

        player.Position = player.Position + aim.normalized * range*Constants.cTileResolution;

        return true;
    }
}
