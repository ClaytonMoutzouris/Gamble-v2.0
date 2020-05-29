using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureGadget : Gadget
{

    //Range of the teleporter in tiles
    public CapturePod pod;
    public EntityPrototype podProto;

    public override bool Activate(Player player, int index)
    {
        Debug.Log("Attempting to active teleporter");
        if (!base.Activate(player, index))
        {
            return false;
        }

        if(pod != null)
        {
            pod.Die();
        }

        pod = new CapturePod(podProto, this);
        pod.Spawn(player.Position);

        return true;
    }


}
