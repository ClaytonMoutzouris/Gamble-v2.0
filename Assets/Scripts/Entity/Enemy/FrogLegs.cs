using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogLegs : Enemy
{

    public FrogLegs(EnemyPrototype proto) : base(proto)
    {

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
                    Vector2 positionVector = Target.Position - Position;

                    if (positionVector.x > 0)
                    {

                        mDirection = EntityDirection.Right;
                    }
                    else
                    {

                        mDirection = EntityDirection.Left;
                    }



                    if (EnemyBehaviour.TargetInRange(this, Target, 128))
                    {
                        mAttackManager.rangedAttacks[0].Activate((positionVector).normalized + new Vector2(Random.Range(-.3f, .3f), Random.Range(-.3f, .3f)), Position);
                        Body.mSpeed.x = 0;

                    }
                    else
                    {
                        if (Body.mPS.pushesLeft || Body.mPS.pushesRight && Body.mPS.pushesBottom)
                        {
                            EnemyBehaviour.Jump(this, jumpHeight);
                        }

                        if (positionVector.y <= 32 && Body.mPS.onOneWay)
                        {
                            Body.mPS.tmpIgnoresOneWay = true;
                        }

                        Body.mSpeed.x = GetMovementSpeed() * (int)mDirection;

                    }

                    if (Mathf.Abs(positionVector.x) < 16)
                    {
                        Body.mSpeed.x = 0;
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
                    Body.mSpeed.x = GetMovementSpeed() * (int)mDirection;


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

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);
    }

}
