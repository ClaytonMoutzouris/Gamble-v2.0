using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Entity, IContactTrigger
{

    /// <summary>
    /// Draws the aabb and ceiling, ground and wall sensors .
    /// </summary>
    /// 
    [HideInInspector]
    public bool mOpen = false;
    public bool locked = false;

    public Spikes(EntityPrototype proto) : base(proto)
    {
        mEntityType = EntityType.Object;

        Body = new PhysicsBody(this, new CustomAABB(Position, proto.bodySize, new Vector2(0, 0)));

        Body.mIsKinematic = false;

        if (Random.Range(0, 10) >= 8)
        {
            locked = true;
        }
    }

    public void Contact(Entity entity)
    {
        float overlapX = 0, overlapY = 0;
        entity.Body.mAABB.OverlapsSigned2D(Body.mAABB, out overlapX, out overlapY);
        //Debug.Log("Overlap: " + overlapX + ", " + overlapY);

        if (entity.Body.mSpeed.y <= 0 && overlapY <= 3 && overlapY > 0)
        {
            entity.Body.Crush();
        }
    }

    public override void EntityUpdate()
    {
        //Chests dont really need to update their physics, but they do need to keep up to date collision data. Right now this is done in the base update
        base.EntityUpdate();
        //UpdatePhysics();

    }

}
