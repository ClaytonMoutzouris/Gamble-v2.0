using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherable : Entity, IInteractable
{

    int charges;
    LootTable lootTable;

    public Gatherable(GatherablePrototype proto) : base(proto)
    {
        mEntityType = EntityType.Object;

        Body.mIsKinematic = false;

        charges = Random.Range(proto.usesMin, proto.usesMax);
        lootTable = proto.lootable;
    }


    public override void EntityUpdate()
    {
        //Chests dont really need to update their physics, but they do need to keep up to date collision data. Right now this is done in the base update
        base.EntityUpdate();
        //UpdatePhysics();

    }

    public bool Interact(Player actor)
    {

        if(charges <= 0)
        {
            return false;
        }

        foreach (Item item in lootTable.GetLoot())
        {
            ItemObject temp = new ItemObject(ItemDatabase.NewItem(item), Resources.Load("Prototypes/Entity/Objects/ItemObject") as EntityPrototype);
            temp.Spawn(Position);
        }

        charges--;

        if(charges > 0)
        {
            ShowFloatingText("*Tink*", Color.white);
        }
        else
        {
            ShowFloatingText("*Crumble*", Color.white);

        }




        return true;
    }
}
