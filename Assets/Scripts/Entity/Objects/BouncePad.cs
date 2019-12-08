﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : Entity, IContactTrigger
{

    /// <summary>
    /// Draws the aabb and ceiling, ground and wall sensors .
    /// </summary>
    /// 
    [HideInInspector]

    public BouncePad(EntityPrototype proto) : base(proto)
    {
        mEntityType = EntityType.Object;

        Body = new PhysicsBody(this, new CustomAABB(Position, proto.bodySize, new Vector2(0, proto.bodySize.x)));

        Body.mIsKinematic = true;
        Body.mIsHeavy = true;

    }

    public void Contact(Entity entity)
    {

        entity.Body.mSpeed = (Position - entity.Position).normalized * -Constants.cBounceSpeed;
        entity.Body.mPS.isBounce = true;
    }

    public override void EntityUpdate()
    {
        //Chests dont really need to update their physics, but they do need to keep up to date collision data. Right now this is done in the base update
        base.EntityUpdate();

    }

    
}
