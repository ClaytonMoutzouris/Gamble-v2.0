using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState { Idle, Aggrivated, Attack1, Attack2, Attack3 };

public class LavaBoss : Enemy
{
    public Entity target = null;
    public Bullet VolcanicBombPrefab;
    public float AttackTimer = 4.0f;
    public float AttackCooldown = 0.0f;
    public float EruptDuration = 5;
    public float EruptTimer = 0;
    public float ChargeDuration = 8;
    public float ChargeTimer = 0;
    public float ChargeSpeed = 1200;
    public float KnockDuration = 1;
    public float KnockTimer = 0;
    public float KnockedCooldown = 1;
    public bool Knocked = false;
    public int ChargeDirection = 1;
    public float KnockedCoolDownTimer = 0;

    public AudioClip mCrashSFX;
    public AudioClip mEruptSFX;

    public BossState mBossState = BossState.Idle;

    public override void EntityInit()
    {
        base.EntityInit();
        EnemyInit();

        mAnimator = GetComponent<Animator>();

        Body = new PhysicsBody(this, new CustomAABB(transform.position, BodySize, new Vector2(0, BodySize.y), new Vector3(1, 1, 1)));
        HurtBox = new Hurtbox(this, new CustomAABB(transform.position, BodySize, Vector3.zero, new Vector3(1, 1, 1)));
        sight = new Sightbox(this, new CustomAABB(transform.position, new Vector2(150, 150), Vector3.zero, new Vector3(1, 1, 1)));

        Body.mIsKinematic = false;
        //Body.mIgnoresGravity = true;

        HurtBox.UpdatePosition();
        sight.UpdatePosition();

        RangedAttack ranged = new RangedAttack(this, 0.2f, 10, VolcanicBombPrefab);
        mAttackManager.AttackList.Add(ranged);

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

        CollisionManager.UpdateAreas(sight);
        sight.mEntitiesInSight.Clear();


        //HurtBox.mCollisions.Clear();
        //UpdatePhysics();

        //make sure the hitbox follows the object
    }

    private void Charge()
    {
        mAnimator.Play("LavaBossWalk");
        //mAnimator.playbackTime = ;
        Body.mIsKinematic = true;

        if (ChargeTimer >= ChargeDuration)
        {
            Body.mIsKinematic = false;

            mBossState = BossState.Aggrivated;
            ChargeTimer = 0;
            KnockTimer = 0;
            Knocked = false;

            return;
        }

        ChargeTimer += Time.deltaTime;

        if (!Knocked)
        {
            //mMovingSpeed = ChargeSpeed * ChargeDirection;
            body.mSpeed.x = ChargeSpeed * ChargeDirection;
            KnockedCoolDownTimer += Time.deltaTime;
            if (KnockedCoolDownTimer > KnockedCooldown && (body.mPS.pushesLeftTile || body.mPS.pushesRightTile))
            {
                mAudioSource.PlayOneShot(mCrashSFX);
                ChargeDirection = -1 * ChargeDirection;
                Knocked = true;
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

        mAttackManager.AttackList[0].Activate();

    }

    private void Aggrivated()
    {
        mAnimator.Play("LavaBossWalk");
        //This works amazing!

        if (target.Position.x > Position.x)
        {
            mMovingSpeed = Mathf.Abs(mMovingSpeed);
            Body.mAABB.ScaleX = -1;
        }
        else if (target.Position.x < Position.x)
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

    private void Idle()
    {
        mAnimator.Play("LavaBossIdle");

        target = null;
        if (sight.mEntitiesInSight != null)
        {
            foreach (Entity entity in sight.mEntitiesInSight)
            {
                if (entity is Player)
                {
                    target = entity;
                    mBossState = BossState.Aggrivated;
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

    public override void Shoot(Bullet prefab, Attack attack)
    {
        Bullet temp = Instantiate(prefab, body.mAABB.Center, Quaternion.identity);
        temp.EntityInit();
        temp.Attack = attack;
        temp.Owner = this;
        temp.SetInitialDirection(new Vector2(UnityEngine.Random.Range(-.7f, .7f), 1));
        
    }
}
