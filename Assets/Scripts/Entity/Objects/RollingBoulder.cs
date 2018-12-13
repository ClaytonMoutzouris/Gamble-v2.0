
using UnityEngine;

public class RollingBoulder : PhysicsObject
{
    public float mMovingSpeed = 1;
    public float mMaxMoveSpeed = 150.0f;
    public float mDir = 1;

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
        mAABB.HalfSize = new Vector2(31.0f, 31.0f);
        mIsKinematic = true;
        int r = Random.Range(0, 2);
        if (r == 1)
        {
            mSpeed.x = mMovingSpeed;

        }
        else
        {
            mSpeed.x = -mMovingSpeed;

        }


        base.ObjectInit();
    }

    public override void CustomUpdate()
    {
        if (mMovingSpeed < mMaxMoveSpeed)
            mMovingSpeed += Time.deltaTime * 100;

        if (mPS.pushesRightTile)
        {
            mDir = -1;
            ScaleX = -Mathf.Abs(ScaleX);
        }
        else if (mPS.pushesLeftTile)
        {
            mDir = 1;
            ScaleX = Mathf.Abs(ScaleX);

        }

        mSpeed.x = mMovingSpeed * mDir;

        if (!mPS.pushesBottom)
        {
            mSpeed.y += Constants.cGravity * Time.deltaTime;

            mSpeed.y = Mathf.Max(mSpeed.y, Constants.cMaxFallingSpeed);
        }
        UpdatePhysics();
    }
}
