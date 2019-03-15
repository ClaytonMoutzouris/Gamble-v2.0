using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : Enemy
{

    public override void EntityInit()
    {
        base.EntityInit();
        //Body.mAABB.ScaleX *= -1;
        body.mAABB.baseOffset = Vector3.zero;
        body.mAABB.Offset = Vector3.zero;


        body.mIsKinematic = true;
        body.mIgnoresGravity = true;

        EnemyInit();

        mEnemyState = EnemyState.Moving;
        body.mSpeed.x = mMovingSpeed;
    }

    public override void EntityUpdate()
    {
        EnemyUpdate();
        base.EntityUpdate();

        switch (mEnemyState)
        {
            case EnemyState.Idle:

                break;

            case EnemyState.Moving:
                if (body.mPS.pushesLeftTile)
                {
                    body.mSpeed.y = -mMovingSpeed;

                }

                 if (body.mPS.pushesBottomTile)
                {
                    body.mSpeed.x = mMovingSpeed;

                }

                 if (body.mPS.pushesTopTile)
                {
                    body.mSpeed.x = -mMovingSpeed;

                }

                 if (body.mPS.pushesRightTile)
                {
                    body.mSpeed.y = mMovingSpeed;

                }

                //transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, transform.rotation.z + 1));

                break;
        }


        CollisionManager.UpdateAreas(HurtBox);
        CollisionManager.UpdateAreas(Sight);
        Sight.mEntitiesInSight.Clear();
    }

}
