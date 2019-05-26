using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HedgehogBoss : Enemy
{
    #region SetInInspector
    public BossState mBossState = BossState.Idle;

    public float AttackTimer = 4.0f;
    public float EruptDuration = 5;
    public float ChargeDuration = 8;
    public float ChargeSpeed = 1200;
    public float KnockDuration = 1;
    public float KnockedCooldown = 1;

    public AudioClip mCrashSFX;
    public AudioClip mEruptSFX;

    public Projectile VolcanicBombPrefab;
    #endregion


    private float AttackCooldown = 0.0f;
    private float EruptTimer = 0;
    private float ChargeTimer = 0;
    private float KnockTimer = 0;
    private bool Knocked = false;
    private int ChargeDirection = 1;
    private float KnockedCoolDownTimer = 0;

    public bool bossTrigger = false;



    public override void EntityInit()
    {
        base.EntityInit();



        Body.mIsKinematic = false;
        Body.mIsHeavy = false;
        //Body.mIgnoresGravity = true;



        
        EnemyInit();
        mAttackManager.AttackList.Clear();
        MeleeAttack meleeAttack = new MeleeAttack(this, .1f, 6, .1f, new Hitbox(this, new CustomAABB(transform.position, Body.mAABB.HalfSize, Vector3.zero, new Vector3(1.1f, 1.1f, 1.1f))));
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

    public void TriggerBoss()
    {
        if (bossTrigger)
            return;

        bossTrigger = true;
        mBossState = BossState.Aggrivated;

        SoundManager.instance.PlayMusic(2);
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

    private void Idle()
    {
        mAnimator.Play("Idle");
        CheckForTargets();
    }

    public void CheckForTargets()
    {
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

    public override void Die()
    {
        SoundManager.instance.PlayMusic(1);

        base.Die();
    }
}
