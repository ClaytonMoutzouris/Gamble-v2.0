using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stag : Enemy
{

    public Stag(EnemyPrototype proto) : base(proto)
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

                    if (!mAttackManager.meleeAttacks[0].OnCooldown())
                    {

                        mMovingSpeed = prototype.movementSpeed * 2;

                        if (Body.mPS.pushesLeftTile || Body.mPS.pushesRightTile && Body.mPS.pushesBottom && positionVector.y > 0)
                        {
                            EnemyBehaviour.Jump(this, jumpHeight);
                        }

                        if (EnemyBehaviour.TargetInRange(this, Target, 64))
                        {
                            mAttackManager.meleeAttacks[0].Activate();
                        }

                    }
                    else
                    {
                        mMovingSpeed = prototype.movementSpeed;
                    }

                    if (positionVector.y <= 32 && Body.mPS.onOneWay)
                    {
                        Body.mPS.tmpIgnoresOneWay = true;
                    }

                    EnemyBehaviour.MoveHorizontal(this);

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


                }


                break;
            case EnemyState.Jumping:
                if (Body.mPS.pushesBottom)
                {
                    mEnemyState = EnemyState.Moving;
                    break;
                }
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

                    if (!mAttackManager.meleeAttacks[0].OnCooldown())
                    {

                        mMovingSpeed = prototype.movementSpeed * 2;
                        EnemyBehaviour.MoveHorizontal(this);

                        if (EnemyBehaviour.TargetInRange(this, Target, 64))
                        {
                            mAttackManager.meleeAttacks[0].Activate();

                        }

                    }
                    else
                    {
                        mMovingSpeed = prototype.movementSpeed;
                    }
                }

                break;
        }


        if (mAttackManager.meleeAttacks[0].mIsActive)
        {
            Renderer.SetAnimState("Attack1");

        }
        else
        {
            Renderer.SetAnimState("Idle");

        }

        base.EntityUpdate();

    }

}
