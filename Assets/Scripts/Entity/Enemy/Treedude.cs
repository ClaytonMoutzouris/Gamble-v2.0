﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treedude : Enemy
{
    public Treedude(EnemyPrototype proto) : base(proto)
    {

        Body.mSpeed.x = GetMovementSpeed();
        Body.mIsKinematic = false;


    }

    public override void EntityUpdate()
    {

        EnemyBehaviour.CheckForTargets(this);

        switch (mEnemyState)
        {
            case EnemyState.Idle:

                break;
            case EnemyState.Moving:

                if (Target != null)
                {
                    //Replace this with pathfinding to the target

                    if (EnemyBehaviour.TargetInRange(this, Target, 20))
                    {
                        mAttackManager.meleeAttacks[0].Activate();
                    }
                    else
                    {

                        if (Target.Position.x > Position.x)
                        {
                            if (Body.mPS.pushesRightTile && Body.mPS.pushesBottom)
                            {
                                EnemyBehaviour.Jump(this, jumpHeight);
                            }
                            mMovingSpeed = Mathf.Abs(mMovingSpeed);
                        }
                        else
                        {
                            if (Body.mPS.pushesLeftTile && Body.mPS.pushesBottom)
                            {
                                EnemyBehaviour.Jump(this, jumpHeight);
                            }
                            mMovingSpeed = -Mathf.Abs(mMovingSpeed);
                        }

                        if (Mathf.Abs(Target.Position.x - Position.x) < 16)
                        {
                            Body.mSpeed.x = 0;
                        }
                    }

                }
                else
                {
                    if (Body.mPS.pushedLeftTile)
                    {

                        mMovingSpeed = Mathf.Abs(mMovingSpeed);

                    }
                    else if (Body.mPS.pushedRightTile)
                    {
                        mMovingSpeed = -Mathf.Abs(mMovingSpeed);
                    }


                }

                Body.mSpeed.x = GetMovementSpeed();

                break;
            case EnemyState.Jumping:
                if (Body.mPS.pushesBottom)
                {
                    mEnemyState = EnemyState.Moving;
                    break;
                }

                if (Target != null)
                {
                    //Replace this with pathfinding to the target

                    if (EnemyBehaviour.TargetInRange(this, Target, 20))
                    {
                        mAttackManager.meleeAttacks[0].Activate();
                    }
                    else
                    {

                        if (Target.Position.x > Position.x)
                        {
                            mMovingSpeed = Mathf.Abs(mMovingSpeed);
                        }
                        else
                        {
                            mMovingSpeed = -Mathf.Abs(mMovingSpeed);
                        }
                    }

                }

                Body.mSpeed.x = GetMovementSpeed();


                break;
        }

        if (Body.mSpeed.x > 0)
        {
            mDirection = EntityDirection.Right;
            //Body.mAABB.ScaleX = 1;
        }
        else
        {
            mDirection = EntityDirection.Left;
            //Body.mAABB.ScaleX = -1;

        }

        base.EntityUpdate();
        //HurtBox.mCollisions.Clear();

        //make sure the hitbox follows the object
    }
}
