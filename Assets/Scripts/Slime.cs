using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MovingObject {

    public float mMovingSpeed;

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
        mAABB.HalfSize = new Vector2(10.0f, 5.0f);
        mAABB.Center = mPosition;
        mSlopeWallHeight = 0;
        mMovingSpeed = 50.0f;
        mIsKinematic = false;
        int r = Random.Range(0, 2);
            mSpeed.x = mMovingSpeed;
       
        
        Scale = new Vector2(1.0f, 1.0f);
        mAABB.OffsetY = mAABB.HalfSizeY;

        mUpdateId = mGame.AddToUpdateList(this);


    }

    public void CustomUpdate()
    {
        if (!mPS.pushesBottom)
        {
            mSpeed.y += Constants.cGravity * Time.deltaTime;

            mSpeed.y = Mathf.Max(mSpeed.y, Constants.cMaxFallingSpeed);
        }


        if (!mPS.pushesRightTile && !mPS.pushedLeftTile)
            mSpeed.x = mMovingSpeed;

        if (mPS.pushesLeftTile || mPS.pushesRightTile)
        {
            mMovingSpeed *= -1;

            mSpeed.x = mMovingSpeed;
        }

        UpdatePhysics();
    }
}
