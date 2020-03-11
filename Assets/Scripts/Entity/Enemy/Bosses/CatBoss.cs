using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBoss : BossEnemy
{

    float AttackCooldown = 0;
    float AttackTimer = 1;
    float jumpCooldown = 1;
    float jumpTimer = 0;
    bool jumped = false;

    bool attacked = false;


    public CatBoss(BossPrototype proto) :base(proto)
    {
        Body.mIsKinematic = true;
        Body.mIsHeavy = true;
    }
    
    public override void EntityUpdate()
    {


        Body.mSpeed.x = 0;

        switch (mBossState)
        {
            case BossState.Idle:

                CheckForTargets();

                break;
            case BossState.Aggrivated:
                Aggrivated();
                break;
            case BossState.Attack1:
                IcicleMove();
                break;
            case BossState.Attack2:
                CatScratch();
                break;
        }

        base.EntityUpdate();


        //HurtBox.mCollisions.Clear();
        //UpdatePhysics();

        //make sure the hitbox follows the object
    }

    void Aggrivated()
    {
        CheckForTargets();

        //This works amazing!
        if (Target != null)
        {
            if (!mAttackManager.meleeAttacks[0].OnCooldown() && !mAttackManager.meleeAttacks[0].mIsActive)
            {
                mBossState = BossState.Attack2;
            }
            else
            {
                mBossState = BossState.Attack1;
            }

        }

    }

    void CatScratch()
    {
        if (Target != null && !Target.IsDead)
        {          
        
            

            if (Body.mPS.pushesBottom && !Body.mPS.pushedBottom)
            {
                MeleeAttack attack = mAttackManager.meleeAttacks[0];
                attack.Activate();

                mBossState = BossState.Aggrivated;
                return;
            }
            else if (Body.mPS.pushesBottom && Body.mPS.pushedBottom)
            {
                if(Target.Position.y > Position.y)
                    EnemyBehaviour.Jump(this, jumpHeight);

            }




            if (Vector2.Distance(Target.Position, Position) < 64)
            {
                //Replace this with pathfinding to the target
                if (!mAttackManager.rangedAttacks[0].mIsActive)
                {
                    Debug.Log("Claw Swipe is not active.");

                    MeleeAttack attack = mAttackManager.meleeAttacks[0];
                    attack.Activate();

                    mBossState = BossState.Aggrivated;
                    jumped = false;


                }
                else
                {
                    Debug.Log("Claw Swipe is active.");

                }
            }
            else
            {
                Body.mSpeed.x = GetMovementSpeed() * (int)mDirection;
                if (Body.mPS.pushesLeftTile && !Body.mPS.pushedLeftTile || Body.mPS.pushesRightTile && !Body.mPS.pushedRightTile)
                {
                    mDirection = (EntityDirection)((int)mDirection * -1);
                    mBossState = BossState.Aggrivated;
                }
            }


        }
        else
        {
            mBossState = BossState.Aggrivated;
            jumped = false;
        }

    }

    void IcicleMove()
    {

        if (Target != null && !Target.IsDead)
        {

            if (Target.Position.x > Position.x)
            {
                mDirection = EntityDirection.Right;
            }
            else
            {
                mDirection = EntityDirection.Left;

            }
            //Replace this with pathfinding to the target
            if (!mAttackManager.rangedAttacks[0].mIsActive)
            {

                    Vector2 dir = ((Vector2)Target.Position - Position).normalized;
                    mAttackManager.rangedAttacks[0].Activate(dir, Position);

                    mBossState = BossState.Aggrivated;

            }
        }
        else
        {
            mBossState = BossState.Aggrivated;
        }

    }
}
