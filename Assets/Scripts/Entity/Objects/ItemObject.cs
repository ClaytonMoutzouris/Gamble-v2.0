using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : Entity {


    public override void EntityInit()
    {
        base.EntityInit();
        Body = new PhysicsBody(this, new CustomAABB(transform.position, new Vector2(5.0f, 5.0f), new Vector2(0, 5.0f), new Vector3(1, 1, 1)));
        body.mIsKinematic = false;

    }

    public override void EntityUpdate()
    {

        base.EntityUpdate();

    }

}
