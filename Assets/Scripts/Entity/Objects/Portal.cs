using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : Entity, IInteractable
{

    public Portal sibling = null;
    private string interactLabel = "<Enter>";

    public string InteractLabel { get => interactLabel; set => interactLabel = value; }

    public Portal(EntityPrototype proto) : base(proto)
    {
        mEntityType = EntityType.Object;


        Body.mIsKinematic = false;
        Body.mIgnoresGravity = proto.ignoreGravity;
    }


    public override void EntityUpdate()
    {
        //Chests dont really need to update their physics, but they do need to keep up to date collision data. Right now this is done in the base update
        base.EntityUpdate();
        //UpdatePhysics();

    }

    public void SetSibling(Portal portal)
    {
        sibling = portal;
    }

    public bool Interact(Player actor)
    {
        if(sibling != null)
        {
            actor.Position = sibling.Position;
        }
        else
        {
            return false;
        }

        return true;
    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);

        //Change the door sprite accordingly
    }



}
