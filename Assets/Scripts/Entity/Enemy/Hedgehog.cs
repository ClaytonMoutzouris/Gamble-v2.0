using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hedgehog : Enemy
{
    public Hedgehog(EnemyPrototype proto) : base(proto)
    {

        Body.mSpeed.x = GetMovementSpeed();
        Body.mIsKinematic = false;
        Body.mIsHeavy = false;
        //Body.mAABB.Scale = new Vector3(.5f, .5f, .5f);


        //StartCoroutine(EnemyBehaviour.Wait(this, 2, EnemyState.Moving));

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
                    if (!mAttackManager.meleeAttacks[0].mIsActive)
                    {
                        Renderer.SetAnimState("Idle");

                        if (EnemyBehaviour.TargetInRange(this, Target, 32))
                        {
                            mAttackManager.meleeAttacks[0].Activate();
                        }

                        if (Body.mPS.pushesLeft || Body.mPS.pushesRight && Body.mPS.pushesBottom)
                        {
                            EnemyBehaviour.Jump(this, jumpHeight);
                        }

                        if (positionVector.y <= 32 && Body.mPS.onOneWay)
                        {
                            Body.mPS.tmpIgnoresOneWay = true;
                        }

                    } else
                    {
                        Renderer.SetAnimState("Attack1");

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

                if (!mAttackManager.meleeAttacks[0].mIsActive)
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

                if (!mAttackManager.meleeAttacks[0].mIsActive)
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
