
using UnityEngine;

public class MovingPlatform : Entity
{
    public bool mWait;
    public float mTimer = 0.0f;
    public float mWaitTime = 3.0f;

    public override void EntityInit()
    {
        base.EntityInit();

        mWait = false;
        Body = new PhysicsBody(this, new CustomAABB(transform.position, new Vector2(30.0f, 8.0f), new Vector2(0, 8.0f), new Vector3(1, 1, 1)));

        mMovingSpeed = 100.0f;
        Body.mIsKinematic = true;
        int r = Random.Range(0, 2);
        if(r == 1)
        {
            Body.mSpeed.x = mMovingSpeed;

        }
        else
        {
            Body.mSpeed.y = mMovingSpeed;

        }


        

    }

    public override void EntityUpdate()
    {
        if (Body.mPS.pushesRightTile && !Body.mPS.pushesBottomTile)
            Body.mSpeed.x = -mMovingSpeed;
        else if (Body.mPS.pushesBottomTile && !Body.mPS.pushesLeftTile)
            Body.mSpeed.y = mMovingSpeed;
        else if (Body.mPS.pushesLeftTile && !Body.mPS.pushesTopTile)
            Body.mSpeed.x = mMovingSpeed;
        else if (Body.mPS.pushesTopTile && !Body.mPS.pushesRightTile)
            Body.mSpeed.y = -mMovingSpeed;

        base.EntityUpdate();
    }
}
