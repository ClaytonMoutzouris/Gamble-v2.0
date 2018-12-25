using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : Entity {


    public override void EntityInit()
    {

        body.mCollider.mAABB.HalfSize = new Vector2(5.0f, 5.0f);
        body.mIsKinematic = false;

        base.EntityInit();

    }

    public override void EntityUpdate()
    {

        base.EntityUpdate();

    }

}
