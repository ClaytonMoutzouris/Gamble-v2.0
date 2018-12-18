
using UnityEngine;

public class RollingBoulder : Entity
{
    public float mMaxMoveSpeed = 150.0f;
    public float mDir = 1;

    public override void EntityInit()
    {
        Body.mAABB.HalfSize = new Vector2(31.0f, 31.0f);
        Body.mIsKinematic = true;
        int r = Random.Range(0, 2);
        if (r == 1)
        {
            Body.mSpeed.x = mMovingSpeed;

        }
        else
        {
            Body.mSpeed.x = -mMovingSpeed;

        }


        base.EntityInit();
    }

    public override void EntityUpdate()
    {
        if (mMovingSpeed < mMaxMoveSpeed)
            mMovingSpeed += Time.deltaTime * 100;

        if (Body.mPS.pushesRightTile)
        {
            mDir = -1;
            Body.ScaleX = -Mathf.Abs(Body.ScaleX);
        }
        else if (Body.mPS.pushesLeftTile)
        {
            mDir = 1;
            Body.ScaleX = Mathf.Abs(Body.ScaleX);

        }

        Body.mSpeed.x = mMovingSpeed * mDir;

        if (!Body.mPS.pushesBottom)
        {
            Body.mSpeed.y += Constants.cGravity * Time.deltaTime;

            Body.mSpeed.y = Mathf.Max(Body.mSpeed.y, Constants.cMaxFallingSpeed);
        }

        base.EntityUpdate();
    }
}
