using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HedgehogBoss : BossEnemy
{
    #region SetInInspector

    #endregion
    float AttackCooldown = 0f;
    float AttackTimer = 1f;

    public HedgehogBoss(BossPrototype proto) : base(proto)
    {
        Body.mIsKinematic = true;
        Body.mIsHeavy = true;
        AttackCooldown = 0f;
        AttackTimer = 1f;
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

        //This is just a test, probably dont need to do it this way
        base.EntityUpdate();


        //HurtBox.mCollisions.Clear();
        //UpdatePhysics();

        //make sure the hitbox follows the object
    }

    private void AirSpin()
    {
        Renderer.SetAnimState("Attack1");
        mMovingSpeed = 170;

        //mAnimator.playbackTime = ;
        if (Body.mPS.pushesBottom && !Body.mPS.pushedBottom)
        {
            mBossState = BossState.Aggrivated;
        } else if (Body.mPS.pushesBottom)
        {
            EnemyBehaviour.Jump(this, jumpHeight);

        }

        if(!Body.mPS.pushesBottom)
        {
            mAttackManager.rangedAttacks[0].Activate(Vector2.right*(int)mDirection, Position);
        }

        if (Body.mPS.pushesLeftTile && !Body.mPS.pushedLeftTile || Body.mPS.pushesRightTile && !Body.mPS.pushedRightTile)
        {
            mDirection = (EntityDirection)((int)mDirection * -1);
            mBossState = BossState.Aggrivated;
        }

        mAttackManager.meleeAttacks[0].Activate();

        Body.mSpeed.x = mMovingSpeed * (int)mDirection;

    }

    void GroundSpin()
    {
        CheckForTargets();
        Renderer.SetAnimState("Attack1");
        mMovingSpeed = 230;

        mAttackManager.meleeAttacks[0].Activate();

        Body.mSpeed.x = mMovingSpeed * (int)mDirection;

        if (Body.mPS.pushesLeftTile && !Body.mPS.pushedLeftTile || Body.mPS.pushesRightTile && !Body.mPS.pushedRightTile)
        {
            mDirection = (EntityDirection)((int)mDirection * -1);
            mBossState = BossState.Aggrivated;
        }

    }


    private void Aggrivated()
    {
        mMovingSpeed = 80;
        Renderer.SetAnimState("Idle");
        //This works amazing!
        Body.mSpeed.x = 0;

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
        Renderer.SetAnimState("Idle");
    }
}
