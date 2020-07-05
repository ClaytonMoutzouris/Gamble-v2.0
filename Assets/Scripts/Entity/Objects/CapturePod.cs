using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePod : Entity, IContactTrigger
{

    [HideInInspector]
    CaptureGadget gadget;

    public CapturePod(EntityPrototype proto, CaptureGadget gadget) : base(proto)
    {
        this.gadget = gadget;
        mEntityType = EntityType.Object;

        Body = new PhysicsBody(this, new CustomAABB(Position, proto.bodySize, new Vector2(0, proto.bodySize.x)));

        Body.mIsKinematic = true;

    }

    public void Contact(Entity entity)
    {
        if(entity is Enemy enemy && !(entity is Miniboss) && !(entity is BossEnemy))
        {
            enemy.hostility = Hostility.Friendly;
            enemy.Target = null;
            enemy.mHealth.UpdateHealth();
        }
    }

    public override void EntityUpdate()
    {
        //Chests dont really need to update their physics, but they do need to keep up to date collision data. Right now this is done in the base update
        base.EntityUpdate();
        //UpdatePhysics();

    }

    public override void Die()
    {
        gadget.pod = null;
        base.Die();
    }

}
