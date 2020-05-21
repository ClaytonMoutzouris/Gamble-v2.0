using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : Gadget
{

    //Range of the teleporter in tiles
    public int range;

    public override bool Activate(Player player)
    {
        Debug.Log("Attempting to active teleporter");
        if (!base.Activate(player))
        {
            return false;
        }

        player.Position = player.Position + player.GetAimLeft().normalized * range*Constants.cTileResolution;

        return true;
    }
}
