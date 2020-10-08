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
    public List<Item> possibleLoot;
    public LootTable lootTable;

    public Chest(ChestPrototype proto) : base(proto)
    {
        mEntityType = EntityType.Object;


        Body.mIsKinematic = false;

        locked = false;
        possibleLoot = proto.possibleItems;
        this.lootTable = proto.lootTable;
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
            ItemObject temp;

            int randomIndex = Random.Range(0, possibleLoot.Count);
            temp = new ItemObject(ItemDatabase.NewItem(possibleLoot[randomIndex]), Resources.Load("Prototypes/Entity/Objects/ItemObject") as EntityPrototype);
            temp.Spawn(Position);
            //lootTable.GetLoot();
            if(lootTable != null)
            {
                foreach (Item item in lootTable.GetLoot())
                {

                    temp = new ItemObject(ItemDatabase.NewItem(item), Resources.Load("Prototypes/Entity/Objects/ItemObject") as EntityPrototype);
                    temp.Spawn(Position);
                }
            }

            


        }




        return true;
    }
}
