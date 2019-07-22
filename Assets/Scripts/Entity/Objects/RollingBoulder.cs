
using UnityEngine;

public class RollingBoulder : Entity
{
    //Should be a constant
    public float mMaxMoveSpeed = 150.0f;
    [HideInInspector]
    public float mDir = 1;

    public RollingBoulder(EntityPrototype proto) : base(proto)
    {


        Body.mIsKinematic = true;
        Body.mIsHeavy = true;

        int r = Random.Range(0, 2);
        if (r == 1)
        {
            mDir = 1;
            Body.mAABB.ScaleX = Mathf.Abs(Body.mAABB.ScaleX);

        }
        else
        {
            mDir = -1;
            Body.mAABB.ScaleX = -Mathf.Abs(Body.mAABB.ScaleX);
        }


    }

    public override void EntityUpdate()
    {
        if (mMovingSpeed < mMaxMoveSpeed)
            mMovingSpeed += Time.deltaTime * 100;

        if (Body.mPS.pushesRightTile)
        {
            mDir = -1;
            Body.mAABB.ScaleX = -Mathf.Abs(Body.mAABB.ScaleX);
        }
        else if (Body.mPS.pushesLeftTile)
        {
            mDir = 1;
            Body.mAABB.ScaleX = Mathf.Abs(Body.mAABB.ScaleX);

        }

        Body.mSpeed.x = mMovingSpeed * mDir;

        base.EntityUpdate();
    }
}
