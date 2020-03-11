﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : Enemy
{

    public Snowball(EnemyPrototype proto) : base(proto)
    {

    }

    public override void EntityUpdate()
    {

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
                    Vector2 positionVector = Target.Position - Position;

                    if (positionVector.x > 0)
                    {

                        mDirection = EntityDirection.Right;
                    }
                    else
                    {

                        mDirection = EntityDirection.Left;
                    }

                    //Replace this with pathfinding to the target
                    if (!mAttackManager.rangedAttacks[0].mIsActive)
                    {

                        if (EnemyBehaviour.TargetInRange(this, Target, 256))
                        {
                            mAttackManager.rangedAttacks[0].Activate((positionVector).normalized, Position);
                        }

                        if (Body.mPS.pushesLeft || Body.mPS.pushesRight && Body.mPS.pushesBottom)
                        {
                            EnemyBehaviour.Jump(this, jumpHeight);
                        }

                        if (positionVector.y <= 32 && Body.mPS.onOneWay)
                        {
                            Body.mPS.tmpIgnoresOneWay = true;
                        }


                        Debug.Log("Its Active");
                    }

                }
                else
                {
                    if (Body.mPS.pushedLeftTile)
                    {

                        mDirection = EntityDirection.Right;

                    }
                    else if (Body.mPS.pushedRightTile)
                    {
                        mDirection = EntityDirection.Left;
                    }


                }

                if (!mAttackManager.rangedAttacks[0].mIsActive)
                {
                    Body.mSpeed.x = GetMovementSpeed() * (int)mDirection;
                }
                else
                {
                    Body.mSpeed.x = 0;
                }

                break;
            case EnemyState.Jumping:
                if (Body.mPS.pushesBottom)
                {
                    mEnemyState = EnemyState.Moving;
                    break;
                }

                if (!mAttackManager.rangedAttacks[0].mIsActive)
                {
                    Body.mSpeed.x = GetMovementSpeed() * (int)mDirection;
                }
                else
                {
                    Body.mSpeed.x = 0;
                }

                break;
        }

        base.EntityUpdate();

    }

}
