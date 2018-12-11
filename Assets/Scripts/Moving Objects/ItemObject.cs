using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : PhysicsObject {

    void OnDrawGizmos()
    {

        DrawMovingObjectGizmos();
    }

    /// <summary>
    /// Draws the aabb and ceiling, ground and wall sensors .
    /// </summary>
    /// 
    public void Start()
    {
        if (mUpdateId < 0)
        {
            ObjectInit();
            //mSpeed.x = 0;
        }
    }

    public override void ObjectInit()
    {

        mAABB.HalfSize = new Vector2(5.0f, 5.0f);
        mIsKinematic = false;

        base.ObjectInit();

    }

    public override void CustomUpdate()
    {
        
        UpdatePhysics();

    }

}
