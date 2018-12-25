﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treedude : Enemy
{
    public override void EntityInit()
    {
        body.mCollider.mAABB.HalfSize = new Vector2(20.0f, 40.0f);
        body.mIsKinematic = false;
        body.mSpeed.x = mMovingSpeed;

        //mEnemyType = EnemyType.Slime;



        base.EntityInit();

    }

    public override void EntityUpdate()
    {

        if (Body.mSpeed.x > 0)
        {
            mMovingSpeed = Mathf.Abs(mMovingSpeed);
            Body.mCollider.mAABB.ScaleX = 1;
        }
        else if (Body.mSpeed.x < 0)
        {
            mMovingSpeed = Mathf.Abs(mMovingSpeed) * -1;
            Body.mCollider.mAABB.ScaleX = -1;

        }

        if (body.mPS.pushesLeftTile || body.mPS.pushesRightTile)
        {
            mMovingSpeed *= -1;

        }


        body.mSpeed.x = mMovingSpeed;


        body.UpdatePhysics();

        //make sure the hitbox follows the object
    }
}
