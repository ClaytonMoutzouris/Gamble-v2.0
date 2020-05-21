using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : Gadget
{

    public override bool Activate(Player player)
    {
        if (!base.Activate(player))
        {
            return false;
        }



        return true;
    }
}
