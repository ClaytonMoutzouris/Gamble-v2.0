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

    public Chest(ChestPrototype proto) : base(proto)
    {
        mEntityType = EntityType.Object;

        Body = new PhysicsBody(this, new CustomAABB(Position, proto.bodySize, new Vector2(0, proto.bodySize.x)));

        Body.mIsKinematic = false;

        locked = false;
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
            InventorySlot temp = actor.Inventory.FindKeySlot();
            if (temp != null)
            {
                temp.GetOneItem();
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
            MapData data = MapDatabase.GetMap(MapManager.instance.mCurrentMap.worldType);


            foreach (Item item in MapDatabase.GetMap(MapManager.instance.mCurrentMap.worldType).chestLoot.GetLoot())
            {
                ItemObject temp = new ItemObject(ItemDatabase.NewItem(item), Resources.Load("Prototypes/Entity/Objects/ItemObject") as EntityPrototype);
                temp.Spawn(Position);
            }

        }




        return true;
    }
}
