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

    public void Init()
    {
        mPosition = RoundVector(transform.position);

        mAABB.HalfSize = new Vector2(5.0f, 5.0f);
        mAABB.Center = mPosition;
        mIsKinematic = false;



        Scale = new Vector2(1.0f, 1.0f);
        mAABB.OffsetY = mAABB.HalfSizeY;

        mUpdateId = mGame.AddToUpdateList(this);


    }

    public override void CustomUpdate()
    {
        
        UpdatePhysics();

    }

}
