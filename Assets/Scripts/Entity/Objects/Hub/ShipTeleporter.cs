using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTeleporter : Entity, IInteractable
{

    /// <summary>
    /// Draws the aabb and ceiling, ground and wall sensors .
    /// </summary>
    /// 
    private string interactLabel = "<Warp>";


    public ShipTeleporter(EntityPrototype proto) : base(proto)
    {
        mEntityType = EntityType.Object;

        Body.mIsKinematic = false;

    }

    public string InteractLabel { get => interactLabel; set => interactLabel = value; }

    public override void EntityUpdate()
    {
        //Chests dont really need to update their physics, but they do need to keep up to date collision data. Right now this is done in the base update
        base.EntityUpdate();
        //UpdatePhysics();

    }

    public bool Interact(Player actor)
    {
        if(Map.mCurrentMap.Data.mapType == MapType.Hub)
        {
            GameManager.instance.TravelToShip();
            return true;

        }

        if(Map.mCurrentMap.Data.mapType == MapType.Ship)
        {
            int index = WorldManager.instance.currentWorldIndex;

            if(index >= 0)
            {
                GameManager.instance.TravelToWorld(index);
                return true;
            }

        }

        return false;
    }
}