using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBoss : Enemy
{
    #region SetInInspector
    public BossState mBossState = BossState.Idle;
    public Projectile iciclePrefab;
    public bool bossTrigger = false;

    #endregion




    public override void EntityInit()
    {
        base.EntityInit();

        body.mIsKinematic = true;


        EnemyInit();

        mAttackManager.AttackList.Clear();
        mEnemyState = EnemyState.Idle;
        RangedAttack ranged = new RangedAttack(this, 0.1f, 10, 0.5f, Range.Far, iciclePrefab);
        mAttackManager.AttackList.Add(ranged);
    }

    public override void EntityUpdate()
    {

        EnemyUpdate();
        base.EntityUpdate();



        switch (mEnemyState)
        {
            case EnemyState.Idle:

                this.Target = null;
                if (Sight.mEntitiesInSight != null)
                {
                    foreach (Entity entity in Sight.mEntitiesInSight)
                    {
                        if (entity is Player)
                        {
                            this.Target = entity;
                            TriggerBoss();
                            break;
                        }
                    }
                }

                break;
            case EnemyState.Moving:

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
        //UpdatePhysics();

        //make sure the hitbox follows the object
    }

    public void TriggerBoss()
    {
        if (bossTrigger)
            return;

        mEnemyState = EnemyState.Moving;

        SoundManager.instance.PlayMusic(2);
    }


    public override void SecondUpdate()
    {
        base.SecondUpdate();

    }

    void NoTargetUpdate()
    {
        if (Body.mSpeed.x > 0)
        {
            mMovingSpeed = Mathf.Abs(mMovingSpeed);
            Body.mAABB.ScaleX = -1;
        }
        else if (Body.mSpeed.x < 0)
        {
            mMovingSpeed = Mathf.Abs(mMovingSpeed) * -1;
            Body.mAABB.ScaleX = 1;

        }




        body.mSpeed.x = mMovingSpeed;
    }

}
