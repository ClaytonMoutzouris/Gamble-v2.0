using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treedude : Enemy
{
    public override void EntityInit()
    {

        base.EntityInit();



        body.mSpeed.x = mMovingSpeed;
        Body.mIsKinematic = false;
        Body.mIsHeavy = true;
        //Body.mAABB.Scale = new Vector3(.5f, .5f, .5f);



        EnemyInit();

        mBehaviour.canJump = false;

        mBehaviour.moveDuration = 0.5f;
        mBehaviour.cooldownDuration = 0.5f;
        mBehaviour.jumpDuration = 3.0f;

        mBehaviour.cooldownTimer = 0f;
        mBehaviour.moveTimer = 0f;
        mBehaviour.jumpTimer = 0f;
        mBehaviour.direction = 1;
    }

    public override void EntityUpdate()
    {
        EnemyUpdate();

        if (Body.mSpeed.x > 0)
        {
            mMovingSpeed = Mathf.Abs(mMovingSpeed);
            Body.mAABB.ScaleX = 1;
        }
        else if (Body.mSpeed.x < 0)
        {
            mMovingSpeed = Mathf.Abs(mMovingSpeed) * -1;
            Body.mAABB.ScaleX = -1;

        }

        if (body.mPS.pushesLeftTile || body.mPS.pushesRightTile)
        {
            mMovingSpeed *= -1;

        }


        body.mSpeed.x = mMovingSpeed;


        base.EntityUpdate();

        CollisionManager.UpdateAreas(HurtBox);
        CollisionManager.UpdateAreas(Sight);
        Sight.mEntitiesInSight.Clear();

        //HurtBox.mCollisions.Clear();

        //make sure the hitbox follows the object
    }
}
