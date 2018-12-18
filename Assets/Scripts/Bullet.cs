using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity {

    public Vector2 direction = Vector2.zero;
    public float mMaxTime = 10;
    public float mTimeAlive = 0;

    public override void EntityInit()
    {
        Body.mAABB.HalfSize = new Vector2(5.0f, 5.0f);
        Body.mIsKinematic = true;
        mMovingSpeed = 100;
        body.mIgnoresGravity = true;

        base.EntityInit();
    }

    public override void EntityUpdate()
    {
        if(mTimeAlive >= mMaxTime)
        {
            mToRemove = true;
        }

        mTimeAlive += Time.deltaTime;

        Body.mSpeed = mMovingSpeed * direction;

        base.EntityUpdate();

    }

}
