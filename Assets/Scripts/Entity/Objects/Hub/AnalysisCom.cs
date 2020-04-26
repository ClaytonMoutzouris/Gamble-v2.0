using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalysisCom : Entity, IInteractable
{

    /// <summary>
    /// Draws the aabb and ceiling, ground and wall sensors .
    /// </summary>
    /// 
    [HideInInspector]
    public bool mOpen = false;

    public AnalysisCom(EntityPrototype proto) : base(proto)
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

        foreach(InventorySlot slot in actor.Inventory.slots)
        {
            if(slot.item is Biosample sample && !sample.identified)
            {
                sample.Identify();
            }
        }

        return true;
    }
}
