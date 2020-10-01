using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WurmAlien : Enemy
{

    public Projectile mSlimePrefab;
    public bool attacking = false;

    public WurmAlien(EnemyPrototype proto) : base(proto)
    {


        Body.mSpeed.x = GetMovementSpeed();
        Body.mIsKinematic = false;
        //abilityFlags.SetFlag(AbilityFlag.Heavy, true);
        //Body.mAABB.Scale = new Vector3(.5f, .5f, .5f);




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

                    //Replace this with pathfinding to the target
                    if (!mAttackManager.IsAttacking()) {

                        if (EnemyBehaviour.TargetInRange(this, Target, 64))
                        {
                            mAttackManager.meleeAttacks[0].Activate();
                        }
                        else if (EnemyBehaviour.TargetInRange(this, Target, 256))
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

                if (!mAttackManager.IsAttacking())
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


                if (!mAttackManager.IsAttacking())
                {
                    Body.mSpeed.x = GetMovementSpeed() * (int)mDirection;
                }
                else
                {
                    Body.mSpeed.x = 0;
                }

                if (Target != null)
                {
                    EnemyBehaviour.MoveHorizontal(this);

                }

                break;
        }


        base.EntityUpdate();

        //HurtBox.mCollisions.Clear();

        //make sure the hitbox follows the object
    }
}
