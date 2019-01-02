using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity {

    public Vector2 direction = Vector2.zero;
    public float mMaxTime = 10;
    public float mTimeAlive = 0;

    public override void EntityInit()
    {
        base.EntityInit();

        Body = new PhysicsBody(this, new CustomAABB(transform.position, new Vector2(5.0f, 5.0f), new Vector2(0, 5.0f), new Vector3(1, 1, 1)));
        Body.mIsKinematic = true;
        mMovingSpeed = 100;
        body.mIgnoresGravity = true;

    }

    public override void EntityUpdate()
    {
        if(mTimeAlive >= mMaxTime || Body.mPS.pushesBottom || Body.mPS.pushesTop || Body.mPS.pushesLeft || Body.mPS.pushesRight)
        {
            mToRemove = true;
        }

        mTimeAlive += Time.deltaTime;

        Body.mSpeed = mMovingSpeed * direction;

        base.EntityUpdate();

    }

}
