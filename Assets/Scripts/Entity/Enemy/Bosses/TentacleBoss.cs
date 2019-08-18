using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleBoss : BossEnemy
{
    #region SetInInspector

    public float mSummonCooldown = 6;
    public float mSummonTimer = 0;
    public int mMaxMinions = 6;
    public List<Enemy> mMinions = new List<Enemy>();
    #endregion




    public TentacleBoss(BossPrototype proto) : base(proto)
    {
        Body.mIsKinematic = true;
        Body.mIsHeavy = true;
        Body.mIgnoresGravity = true;

    }

    public override void EntityUpdate()
    {

        base.EntityUpdate();

        mSummonTimer += Time.deltaTime;


        switch (mBossState)
        {
            case BossState.Idle:

                CheckForTargets();

                break;
            case BossState.Aggrivated:
                CheckForTargets();

                int mins = 0;

                if (mSummonTimer >= mSummonCooldown)
                {
                    Vector2i tilePos = MapManager.instance.GetMapTileAtPoint(Body.mAABB.Center);
                    Enemy temp = MapManager.instance.AddEnemyEntity(new EnemyData(tilePos.x, tilePos.y, EnemyType.Eye));
                    temp.mEnemyState = EnemyState.Moving;
                    mSummonTimer = 0;
                }

                if (Target != null)
                {
                    Vector2 dir = ((Vector2)Target.Position - Position).normalized;

                    if (!mAttackManager.rangedAttacks[0].onCooldown)
                    {
                        RangedAttack attack = mAttackManager.rangedAttacks[0];
                        attack.Activate(dir);
                    }

                    Body.mSpeed = dir * mMovingSpeed;

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

                    Body.mSpeed.x = mMovingSpeed;

                }


                break;
            case BossState.Attack1:
                CheckForTargets();

                if (Target != null)
                {
                    //Replace this with pathfinding to the target

                    if (!mAttackManager.rangedAttacks[0].onCooldown)
                    {
                        Vector2 dir = ((Vector2)Target.Position - Position).normalized;
                        RangedAttack attack = mAttackManager.rangedAttacks[0];
                        attack.Activate(dir);
                    }


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


                }
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


        CollisionManager.UpdateAreas(HurtBox);

        CollisionManager.UpdateAreas(Sight);
        Sight.mEntitiesInSight.Clear();


        //HurtBox.mCollisions.Clear();
        //UpdatePhysics();

        //make sure the hitbox follows the object
    }
}
