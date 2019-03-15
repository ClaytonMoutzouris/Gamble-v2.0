using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : Enemy
{

    

    public override void EntityInit()
    {
        base.EntityInit();


        Body.mIsKinematic = false;
        Body.mIgnoresGravity = true;
        Body.mIgnoresOneWay = true;


        EnemyInit();
        StartCoroutine(EnemyBehaviour.Wait(this, 2, EnemyState.Moving));


    }

    public override void EntityUpdate()
    {
        EnemyUpdate();
        //This is just a test, probably dont need to do it this way
        base.EntityUpdate();

        if (Hostility == Hostility.Hostile)
        {
            EnemyBehaviour.CheckForTargets(this);
        }

        switch (mEnemyState)
        {
            case EnemyState.Idle:

                break;
            case EnemyState.Moving:

                if (Target != null)
                {
                    mAnimator.Play("Eye_Fly");
                    if(body.mPS.pushesLeftTile || body.mPS.pushesRightTile)
                    {
                        if(Target.Position.y < Position.y)
                        {
                            body.mSpeed.y = -mMovingSpeed;
                        } else
                        {
                            body.mSpeed.y = mMovingSpeed;
                        }
                    } else if (body.mPS.pushesBottomTile || body.mPS.pushesTopTile)
                    {
                        if (Target.Position.x < Position.x)
                        {
                            body.mSpeed.x = -mMovingSpeed;
                        }
                        else
                        {
                            body.mSpeed.x = mMovingSpeed;
                        }
                    } else
                    {
                        body.mSpeed = ((Vector2)Target.Position - body.mPosition).normalized * mMovingSpeed;
                    }

                }
                else
                {
                    if (!body.mPS.pushesTop)
                    {
                        mAnimator.Play("Eye_Fly");
                        body.mSpeed.y = mMovingSpeed;
                    }
                    else
                    {
                        mAnimator.Play("Eye_Sleep");
                        body.mSpeed.y = 0;
                    }

                    body.mSpeed.x = 0;


                }

                break;

        }


        CollisionManager.UpdateAreas(HurtBox);
        CollisionManager.UpdateAreas(Sight);
        Sight.mEntitiesInSight.Clear();


        //HurtBox.mCollisions.Clear();
        //UpdatePhysics();

        //make sure the hitbox follows the object
    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();

    }

}
