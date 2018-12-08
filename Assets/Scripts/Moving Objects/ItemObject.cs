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
            Init();
            //mSpeed.x = 0;
        }
    }

    public override void Init()
    {

        mAABB.HalfSize = new Vector2(5.0f, 5.0f);
        mIsKinematic = false;

        base.Init();

    }

    public override void CustomUpdate()
    {
        
        UpdatePhysics();

    }

}
