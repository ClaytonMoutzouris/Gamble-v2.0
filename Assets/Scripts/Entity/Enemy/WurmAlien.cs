﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WurmAlien : Enemy
{

    public Projectile mSlimePrefab;

    public override void EntityInit()
    {

        base.EntityInit();
        EnemyInit();



        body.mSpeed.x = mMovingSpeed;
        Body.mIsKinematic = false;
        Body.mIsHeavy = false;
        //Body.mAABB.Scale = new Vector3(.5f, .5f, .5f);
        mAttackManager.AttackList[0] = new RangedAttack(this, 0, 2, .5f, mSlimePrefab);


        StartCoroutine(EnemyBehaviour.Wait(this, 2, EnemyState.Moving));


    }

    public override void EntityUpdate()
    {
        EnemyUpdate();

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
                    //Replace this with pathfinding to the target


                    RangedAttack attack = (RangedAttack)mAttackManager.AttackList[0];

                    attack.Activate((Target.Position - Position).normalized);


                    if (Target.Position.x > body.mPosition.x)
                    {
                        if (body.mPS.pushesRightTile && body.mPS.pushesBottom)
                        {
                            EnemyBehaviour.Jump(this, 460);
                        }
                        mMovingSpeed = Mathf.Abs(mMovingSpeed);
                    }
                    else
                    {
                        if (body.mPS.pushesLeftTile && body.mPS.pushesBottom)
                        {
                            EnemyBehaviour.Jump(this, 460);
                        }
                        mMovingSpeed = -Mathf.Abs(mMovingSpeed);
                    }
                    
                }
                else
                {
                    if (body.mPS.pushedLeftTile)
                    {

                        mMovingSpeed = Mathf.Abs(mMovingSpeed);

                    }
                    else if (body.mPS.pushedRightTile)
                    {
                        mMovingSpeed = -Mathf.Abs(mMovingSpeed);
                    }


                }

                body.mSpeed.x = mMovingSpeed;

                break;
            case EnemyState.Jumping:
                if (body.mPS.pushesBottom)
                {
                    mEnemyState = EnemyState.Moving;
                    break;
                }

                if (Target != null)
                {
                    //Replace this with pathfinding to the target

                    if (EnemyBehaviour.TargetInRange(this, Target, 20))
                    {
                        mAttackManager.AttackList[0].Activate();
                    }
                    else
                    {

                        if (Target.Position.x > body.mPosition.x)
                        {
                            mMovingSpeed = Mathf.Abs(mMovingSpeed);
                        }
                        else
                        {
                            mMovingSpeed = -Mathf.Abs(mMovingSpeed);
                        }
                    }

                }

                body.mSpeed.x = mMovingSpeed;


                break;
        }

        if (body.mSpeed.x > 0)
        {
            body.mAABB.ScaleX = 1;
        }
        else
        {
            body.mAABB.ScaleX = -1;

        }

        CollisionManager.UpdateAreas(HurtBox);
        CollisionManager.UpdateAreas(Sight);
        Sight.mEntitiesInSight.Clear();

        //HurtBox.mCollisions.Clear();

        //make sure the hitbox follows the object
    }
}