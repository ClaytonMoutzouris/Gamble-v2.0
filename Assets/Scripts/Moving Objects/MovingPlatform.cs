
using UnityEngine;

public class MovingPlatform : PhysicsObject
{
    public float mMovingSpeed;
    public bool mWait;
    public float mTimer = 0.0f;
    public float mWaitTime = 3.0f;

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
        mWait = false;
        mPosition = transform.position;

        mAABB.HalfSize = new Vector2(30.0f, 8.0f);
        mAABB.Center = mPosition;
        mMovingSpeed = 100.0f;
        mIsKinematic = true;
        int r = Random.Range(0, 2);
        if(r == 1)
        {
            mSpeed.x = mMovingSpeed;

        }
        else
        {
            mSpeed.y = mMovingSpeed;

        }
        Scale = new Vector2(1.0f, 1.0f);

        mUpdateId = mGame.AddToUpdateList(this);

        
    }

    public override void CustomUpdate()
    {
        if (mPS.pushesRightTile && !mPS.pushesBottomTile)
            mSpeed.x = -mMovingSpeed;
        else if (mPS.pushesBottomTile && !mPS.pushesLeftTile)
            mSpeed.y = mMovingSpeed;
        else if (mPS.pushesLeftTile && !mPS.pushesTopTile)
            mSpeed.x = mMovingSpeed;
        else if (mPS.pushesTopTile && !mPS.pushesRightTile)
            mSpeed.y = -mMovingSpeed;

        UpdatePhysics();
    }
}
