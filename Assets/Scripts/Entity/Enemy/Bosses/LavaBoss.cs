using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBoss : BossEnemy
{
    #region SetInInspector

    public float EruptDuration = 2;
    private float EruptTimer = 0;

    public float KnockDuration = 0.5f;
    public float KnockedCooldown = .2f;
    private float KnockedCoolDownTimer = 0;
    private float KnockTimer = 0;
    private bool Knocked = false;

    public AudioClip mCrashSFX;
    public AudioClip mEruptSFX;
    #endregion




    private float ChargeTimer = 0;
    public float ChargeDuration = 2;
    public float ChargeSpeed = 1200;
    private int ChargeDirection = 1;




    public LavaBoss(BossPrototype proto) : base(proto)
    {
        Body.mIsKinematic = true;
        Body.mIsHeavy = true;


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
        Renderer.SetAnimState("LavaBossWalk");
        //mAnimator.playbackTime = ;

        if (ChargeTimer >= ChargeDuration)
        {

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
            Body.mSpeed.x = ChargeSpeed * ChargeDirection;
            KnockedCoolDownTimer += Time.deltaTime;
            if (KnockedCoolDownTimer > KnockedCooldown && (Body.mPS.pushesLeftTile || Body.mPS.pushesRightTile))
            {
                SoundManager.instance.PlaySingle(mCrashSFX);
                ChargeDirection = -1 * ChargeDirection;
                for(int i = 0; i < 3; i++)
                {
                    Vector2i spawnPoint = MapManager.instance.GetRoofTile(new Vector2i(Random.Range(4, Map.mCurrentMap.getMapSize().x-4), 1));
                    mAttackManager.rangedAttacks[1].Activate(Vector2.zero, MapManager.instance.GetMapTilePosition(spawnPoint));
                    mAttackManager.rangedAttacks[1].Deactivate();


                }
                Knocked = true;
                KnockTimer = 0;
                mDirection = (EntityDirection)((int)mDirection * -1);
                //Body.mAABB.ScaleX *= -1;

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
            Body.mSpeed.x = 0;
        }
        

    }

    private void Erupt()
    {
        Body.mSpeed = Vector2.zero;

        Renderer.SetAnimState("LavaBossErupt");

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

        mAttackManager.rangedAttacks[0].Activate(Vector2.up, Position);

    }

    private void Aggrivated()
    {
        Renderer.SetAnimState("LavaBossWalk");
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
            mDirection = EntityDirection.Right;
            //Body.mAABB.ScaleX = -1;
        }
        else if (Target.Position.x < Position.x)
        {
            mMovingSpeed = Mathf.Abs(mMovingSpeed) * -1;
            mDirection = EntityDirection.Left;
            //Body.mAABB.ScaleX = 1;
        }

        Body.mSpeed.x = mMovingSpeed;

        int randomAttack = UnityEngine.Random.Range(0, 2);
        if(randomAttack == 1)
        {
            mBossState = BossState.Attack1;
        } else
        {
            mBossState = BossState.Attack2;
        }
            



        //body.mSpeed = ((Vector2)target.Position - body.mPosition).normalized * Mathf.Abs(mMovingSpeed);
    }

    protected override void Idle()
    {
        base.Idle();
        Renderer.SetAnimState("LavaBossIdle");
    }

}
