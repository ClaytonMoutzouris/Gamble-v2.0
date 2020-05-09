using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Entity, IInteractable
{

    /// <summary>
    /// Draws the aabb and ceiling, ground and wall sensors .
    /// </summary>
    /// 
    [HideInInspector]
    public bool locked = false;
    


    public Door(EntityPrototype proto) : base(proto)
    {
        mEntityType = EntityType.Object;

        Body = new PhysicsBody(this, new CustomAABB(Position, proto.bodySize, new Vector2(0, proto.bodySize.x)));

        Body.mIsKinematic = false;

    }


    public override void EntityUpdate()
    {
        //Chests dont really need to update their physics, but they do need to keep up to date collision data. Right now this is done in the base update
        base.EntityUpdate();
        //UpdatePhysics();

    }

    public bool Interact(Player actor)
    {
        if(locked)
        {
            InventorySlot temp = actor.Inventory.FindKeySlot();
            if (temp != null)
            {
                temp.GetOneItem();
                locked = false;
                ShowFloatingText("Unlocked!", Color.green);
                return true;
            } else
            {
                ShowFloatingText("Locked", Color.red);
                return false;
            }
        } else
        {
            Game.mMapChangeFlag = true;

        }

        return true;
    }
    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);

        //Change the door sprite accordingly
    }
    

    
}
