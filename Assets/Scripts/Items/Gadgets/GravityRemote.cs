using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityRemote : Gadget
{

    //Range of the teleporter in tiles
    bool isOn = false;

    public override bool Activate(Player player, int index)
    {
        Debug.Log("Attempting to active gravity remote");
        if (!base.Activate(player, index))
        {
            return false;
        }

        if(isOn)
        {
            isOn = false;
            player.gravityVector = Vector2.down;
        } else
        {
            isOn = true;
            player.gravityVector = Vector2.up;
        }

        return true;
    }
}
