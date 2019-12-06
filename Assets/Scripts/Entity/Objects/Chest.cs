using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Entity, IInteractable {

    /// <summary>
    /// Draws the aabb and ceiling, ground and wall sensors .
    /// </summary>
    /// 
    [HideInInspector]
    public bool mOpen = false;
    public bool locked = false;

    public Chest(EntityPrototype proto) : base(proto)
    {
        mEntityType = EntityType.Object;

        Body = new PhysicsBody(this, new CustomAABB(Position, proto.bodySize, new Vector2(0, proto.bodySize.x)));

        Body.mIsKinematic = false;

        if(Random.Range(0, 10) >= 8)
        {
            locked = true;
        }
    }


    public override void EntityUpdate()
    {
        //Chests dont really need to update their physics, but they do need to keep up to date collision data. Right now this is done in the base update
        base.EntityUpdate();
        //UpdatePhysics();

    }

    public bool Interact(Player actor)
    {
        if (mOpen)
            return true;

        if(locked)
        {
            Item temp = actor.Inventory.GetKeyItem();
            if (temp != null)
            {
                actor.Inventory.RemoveItem(temp);
                locked = false;
                ShowFloatingText("Unlocked!", Color.green);
                return true;
            }
            else
            {
                ShowFloatingText("Locked", Color.red);
                return false;
            }
        } else
        {
            mOpen = true;
            Renderer.SetAnimState("ChestOpen");

            ItemObject temp = new ItemObject(ItemDatabase.GetRandomItem(), Resources.Load("Prototypes/Entity/Objects/ItemObject") as EntityPrototype);
            temp.Spawn(Position);
        }




        return true;
    }
}
