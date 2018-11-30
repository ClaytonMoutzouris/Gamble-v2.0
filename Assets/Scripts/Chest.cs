using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : PhysicsObject {

    /// <summary>
    /// Draws the aabb and ceiling, ground and wall sensors .
    /// </summary>
    /// 
    public Animator mAnimator;
    public bool mOpen = false;
    public bool mWasOpen = false;
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
        SetTilePosition(mMap.GetMapTileAtPoint(transform.position));
        //mPosition = RoundVector(transform.position);

        mAABB.HalfSize = new Vector2(10.0f, 10.0f);
        //mAABB.Center = mPosition;
        mIsKinematic = false;



        Scale = new Vector2(1.0f, 1.0f);
        mAABB.OffsetY = mAABB.HalfSizeY;

        mUpdateId = mGame.AddToUpdateList(this);


    }

    public override void CustomUpdate()
    {
        if(mOpen)
        {
            if (!mWasOpen)
            {
                mAnimator.Play("ChestOpen");
            }
            mWasOpen = true;
        }
        //UpdatePhysics();

    }
}
