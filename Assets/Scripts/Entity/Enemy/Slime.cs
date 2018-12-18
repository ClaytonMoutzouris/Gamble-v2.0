using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy {


    public override void EntityInit()
    {
        body.mAABB.HalfSize = new Vector2(10.0f, 5.0f);
        body.mIsKinematic = false;
        body.mSpeed.x = mMovingSpeed;

        //mEnemyType = EnemyType.Slime;



        base.EntityInit();

    }

    public override void EntityUpdate()
    {

        if (!body.mPS.pushesRightTile && !body.mPS.pushedLeftTile)
            body.mSpeed.x = mMovingSpeed;

        if (body.mPS.pushesLeftTile || body.mPS.pushesRightTile)
        {
            mMovingSpeed *= -1;

            body.mSpeed.x = mMovingSpeed;
        }

        
        body.UpdatePhysics();

        //make sure the hitbox follows the object
    }
}
