using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treedude : Enemy
{
    public override void EntityInit()
    {

        base.EntityInit();


        Body = new PhysicsBody(this, new CustomAABB(transform.position, new Vector2(20.0f, 40.0f), new Vector2(0, 40.0f), new Vector3(1, 1, 1)));
        HurtBox = new Hurtbox(this, new CustomAABB(transform.position, new Vector2(20.0f, 40.0f), Vector3.zero, new Vector3(1, 1, 1)));

        body.mSpeed.x = mMovingSpeed;



        HurtBox.UpdatePosition();

        EnemyInit();

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
        CollisionManager.UpdateAreas(sight);
        sight.mEntitiesInSight.Clear();

        //HurtBox.mCollisions.Clear();

        //make sure the hitbox follows the object
    }
}
