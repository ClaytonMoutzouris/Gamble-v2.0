using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBoss : BossEnemy
{
    #region SetInInspector

    public float EruptDuration = 5;
    public float ChargeDuration = 8;
    public float ChargeSpeed = 1200;
    public float KnockDuration = 1;
    public float KnockedCooldown = 1;

    public AudioClip mCrashSFX;
    public AudioClip mEruptSFX;
    #endregion


    private float EruptTimer = 0;
    private float ChargeTimer = 0;
    private float KnockTimer = 0;
    private bool Knocked = false;
    private int ChargeDirection = 1;
    private float KnockedCoolDownTimer = 0;


    public override void EntityInit()
    {
        base.EntityInit();
        Body.mIsKinematic = true;
        Body.mIsHeavy = true;
        EnemyInit();

        mAttackManager.AttackList.Clear();
        RangedAttack ranged = new RangedAttack(this, 0.05f, 5, 0.1f, projectilePrefabs[0]);
        mAttackManager.AttackList.Add(ranged);
        MeleeAttack meleeAttack = new MeleeAttack(this, 5f, 20, .1f, new Hitbox(this, new CustomAABB(transform.position, Body.mAABB.HalfSize, Vector3.zero, new Vector3(1.1f, 1.1f, 1.1f))));
        mAttackManager.AttackList.Add(meleeAttack);
        mAttackManager.meleeAttacks.Add(meleeAttack);

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
                Erupt();
                break;
            case BossState.Attack2:
                Charge();
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

    private void Charge()
    {
        mAnimator.Play("LavaBossWalk");
        //mAnimator.playbackTime = ;

        if (ChargeTimer >= ChargeDuration)
        {

            mBossState = BossState.Aggrivated;
            ChargeTimer = 0;
            KnockTimer = 0;
            Knocked = false;
            mAttackManager.AttackList[1].Deactivate();
            return;
        }

        ChargeTimer += Time.deltaTime;


        if (!Knocked)
        {
            mAttackManager.AttackList[1].Activate();

            //mMovingSpeed = ChargeSpeed * ChargeDirection;
            body.mSpeed.x = ChargeSpeed * ChargeDirection;
            KnockedCoolDownTimer += Time.deltaTime;
            if (KnockedCoolDownTimer > KnockedCooldown && (body.mPS.pushesLeftTile || body.mPS.pushesRightTile))
            {
                SoundManager.instance.PlaySingle(mCrashSFX);
                ChargeDirection = -1 * ChargeDirection;
                Knocked = true;
                mAttackManager.AttackList[1].Deactivate();
                KnockTimer = 0;
                Body.mAABB.ScaleX *= -1;

            }
        }
        else
        {


            KnockTimer += Time.deltaTime;

            if (KnockTimer >= KnockDuration)
            {

                Knocked = false;
                KnockedCoolDownTimer = 0;
            }
            //Body.mAABB.ScaleX = -1;
            //mMovingSpeed = 0;
            body.mSpeed.x = 0;
        }
        

    }

    private void Erupt()
    {
        body.mSpeed = Vector2.zero;

        mAnimator.Play("LavaBossErupt");

        if (EruptTimer >= EruptDuration)
        {
            mBossState = BossState.Aggrivated;
            EruptTimer = 0;
            return;
        }
        else
        {
            EruptTimer += Time.deltaTime;
        }

        RangedAttack attack = (RangedAttack)mAttackManager.AttackList[0];
        attack.Activate(new Vector2(UnityEngine.Random.Range(-1.0f, 1.0f), 1));

    }

    private void Aggrivated()
    {
        mAnimator.Play("LavaBossWalk");
        //This works amazing!
        //CheckForTargets();
        if(Target == null)
        {
            mBossState = BossState.Idle;
            return;
        }

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

        body.mSpeed.x = mMovingSpeed;

        if (AttackCooldown >= AttackTimer)
        {
            int randomAttack = UnityEngine.Random.Range(0, 2);
            if(randomAttack == 1)
            {
                mBossState = BossState.Attack1;
            } else
            {
                mBossState = BossState.Attack2;
            }
            
            AttackCooldown = 0;
        } else
        {
            AttackCooldown += Time.deltaTime;
        }


        //body.mSpeed = ((Vector2)target.Position - body.mPosition).normalized * Mathf.Abs(mMovingSpeed);
    }

    protected override void Idle()
    {
        base.Idle();
        mAnimator.Play("LavaBossIdle");
    }

}
