using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGadget : Gadget
{

    //Range of the teleporter in tiles
    Portal portalA = null;
    Portal portalB = null;
    public EntityPrototype portalProto;

    public override bool Activate(Player player, int index)
    {
        Debug.Log("Attempting to active teleporter");
        if (!base.Activate(player, index))
        {
            return false;
        }

        if(portalA != null && portalB != null)
        {
            portalA.Destroy();
            portalA = portalB;
            portalB = new Portal(ScriptableObject.Instantiate(portalProto));

            portalA.SetSibling(portalB);
            portalB.SetSibling(portalA);

            portalB.Spawn(player.Position+player.Body.mOffset);
        } else if(portalA != null && portalB == null)
        {
            portalB = new Portal(ScriptableObject.Instantiate(portalProto));

            portalA.SetSibling(portalB);
            portalB.SetSibling(portalA);

            portalB.Spawn(player.Position + player.Body.mOffset);

        }
        else if(portalA == null)
        {
            portalA = new Portal(ScriptableObject.Instantiate(portalProto));
            portalA.Spawn(player.Position + player.Body.mOffset);

        }




        return true;
    }


}
