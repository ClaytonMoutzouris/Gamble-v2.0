using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : Enemy
{

    public Roller(EnemyPrototype proto) : base(proto)
    {
        //Body.mAABB.ScaleX *= -1;
        Body.mAABB.baseOffset = Vector3.zero;
        Body.mAABB.Offset = Vector3.zero;


        Body.mIsKinematic = true;
        Body.mIgnoresGravity = true;

        mEnemyState = EnemyState.Moving;
        Body.mSpeed.x = mMovingSpeed;
    }

    public override void EntityUpdate()
    {
        base.EntityUpdate();

        switch (mEnemyState)
        {
            case EnemyState.Idle:

                break;

            case EnemyState.Moving:
                if (Body.mPS.pushesLeftTile)
                {
                    Body.mSpeed.y = -mMovingSpeed;

                }

                 if (Body.mPS.pushesBottomTile)
                {
                    Body.mSpeed.x = mMovingSpeed;

                }

                 if (Body.mPS.pushesTopTile)
                {
                    Body.mSpeed.x = -mMovingSpeed;

                }

                 if (Body.mPS.pushesRightTile)
                {
                    Body.mSpeed.y = mMovingSpeed;

                }

                //transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, transform.rotation.z + 1));

                break;
        }


        CollisionManager.UpdateAreas(HurtBox);
        CollisionManager.UpdateAreas(Sight);
        Sight.mEntitiesInSight.Clear();
    }

}
