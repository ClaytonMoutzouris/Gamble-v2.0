using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HedgehogBoss : BossEnemy
{
    #region SetInInspector

    #endregion


    public override void EntityInit()
    {
        base.EntityInit();
                Body.mIsKinematic = true;
        Body.mIsHeavy = true;
        EnemyInit();
        
        mAttackManager.AttackList.Clear();
        MeleeAttack meleeAttack = new MeleeAttack(this, .1f, 2, .05f, new Hitbox(this, new CustomAABB(transform.position, Body.mAABB.HalfSize, Vector3.zero, new Vector3(1.1f, 1.1f, 1.1f))));
        mAttackManager.AttackList.Add(meleeAttack);
        mAttackManager.meleeAttacks.Add(meleeAttack);


        //RangedAttack ranged = new RangedAttack(this, 0.05f, 10, 0.1f, VolcanicBombPrefab);
        //mAttackManager.AttackList.Add(ranged);

    }

    public override void EntityUpdate()
    {
        switch (mBossState)
        {
            case BossState.Idle:
                Idle();
                break;
            case BossState.Aggrivated:
                Aggrivated();
                break;
            case BossState.Attack1:
                GroundSpin();
                break;
            case BossState.Attack2:
                AirSpin();
                break;
        }


        mAttackManager.UpdateAttacks();






        //This is just a test, probably dont need to do it this way
        base.EntityUpdate();


        CollisionManager.UpdateAreas(HurtBox);

        CollisionManager.UpdateAreas(Sight);
        Sight.mEntitiesInSight.Clear();


        //HurtBox.mCollisions.Clear();
        //UpdatePhysics();

        //make sure the hitbox follows the object
    }

    private void AirSpin()
    {
        CheckForTargets();
        mAnimator.Play("Attack1");
        //mAnimator.playbackTime = ;
        if (mEnemyState == EnemyState.Jumping && body.mPS.pushesBottom && !body.mPS.pushedBottom)
        {
            mBossState = BossState.Aggrivated;
            mEnemyState = EnemyState.Aggrivated;
        } else if (mEnemyState != EnemyState.Jumping)
        {
            EnemyBehaviour.Jump(this, 400);

        }
        if (Target != null)
        {
            if (Target.Position.x > Position.x)
            {
                mMovingSpeed = Mathf.Abs(mMovingSpeed);
                Body.mAABB.ScaleX = -1;
            }
            else if (Target.Position.x < Position.x)
            {
                mMovingSpeed = Mathf.Abs(mMovingSpeed) * -1;
                Body.mAABB.ScaleX = 1;
            }
        }

        mAttackManager.AttackList[0].Activate();

        body.mSpeed.x = mMovingSpeed;





    }

    void GroundSpin()
    {
        CheckForTargets();
        mAnimator.Play("Attack1");
        mMovingSpeed = 120;

        CheckForTargets();


        if (Target != null)
        {
            if (Target.Position.x > Position.x)
            {
                mMovingSpeed = Mathf.Abs(mMovingSpeed);
                Body.mAABB.ScaleX = -1;
            }
            else if (Target.Position.x < Position.x)
            {
                mMovingSpeed = Mathf.Abs(mMovingSpeed) * -1;
                Body.mAABB.ScaleX = 1;
            }
        }
        mAttackManager.AttackList[0].Activate();

        body.mSpeed.x = mMovingSpeed;

        if (body.mPS.pushesLeftTile || body.mPS.pushesRightTile)
        {
            mBossState = BossState.Aggrivated;
            mMovingSpeed = 80;
        }

    }


    private void Aggrivated()
    {
        mAnimator.Play("Idle");
        //This works amazing!
        body.mSpeed.x = 0;

        if (AttackCooldown >= AttackTimer)
        {
            int randomAttack = UnityEngine.Random.Range(0, 2);
            if (randomAttack == 1)
            {
                mBossState = BossState.Attack1;
            }
            else
            {
                mBossState = BossState.Attack2;
            }

            AttackCooldown = 0;
        }
        else
        {
            AttackCooldown += Time.deltaTime;
        }

        CheckForTargets();

        //body.mSpeed = ((Vector2)target.Position - body.mPosition).normalized * Mathf.Abs(mMovingSpeed);
    }

    protected override void Idle()
    {
        base.Idle();
        mAnimator.Play("Idle");
    }
}
