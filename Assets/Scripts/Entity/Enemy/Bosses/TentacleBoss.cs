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




    public override void EntityInit()
    {
        base.EntityInit();
        Body.mIsKinematic = true;
        Body.mIsHeavy = true;
        Body.mIgnoresGravity = true;
        EnemyInit();
        mAttackManager.AttackList.Clear();
        RangedAttack ranged = new RangedAttack(this, 0.1f, 10, 0.5f, projectilePrefabs[0]);
        mAttackManager.AttackList.Add(ranged);
    }

    public override void EntityUpdate()
    {

        EnemyUpdate();
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
                    Vector2 dir = ((Vector2)Target.Position - Body.mPosition).normalized;

                    if (!mAttackManager.AttackList[0].onCooldown)
                    {
                        RangedAttack attack = (RangedAttack)mAttackManager.AttackList[0];
                        attack.Activate(dir);
                    }

                    body.mSpeed = dir * mMovingSpeed;

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

                    body.mSpeed.x = mMovingSpeed;

                }


                break;
            case BossState.Attack1:
                CheckForTargets();

                if (Target != null)
                {
                    //Replace this with pathfinding to the target

                    if (!mAttackManager.AttackList[0].onCooldown)
                    {
                        Vector2 dir = ((Vector2)Target.Position - Body.mPosition).normalized;
                        RangedAttack attack = (RangedAttack)mAttackManager.AttackList[0];
                        attack.Activate(dir);
                    }


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
        //UpdatePhysics();

        //make sure the hitbox follows the object
    }
}
