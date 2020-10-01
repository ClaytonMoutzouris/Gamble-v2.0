using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salamander : Miniboss
{
    public Salamander(EnemyPrototype proto) : base(proto)
    {
        Body.mIsKinematic = true;

    }

    public override void EntityUpdate()
    {

        EnemyBehaviour.CheckForTargets(this);


        switch (mEnemyState)
        {
            case EnemyState.Idle:

                break;
            case EnemyState.Moving:


                if (Target != null)
                {

                    int randomAttack = Random.Range(0, 2);

                    switch(randomAttack)
                    {
                        case 0:
                            mEnemyState = EnemyState.Attack1;
                            break;
                        case 1:
                            mEnemyState = EnemyState.Attack2;
                            break;
                    }

                }
                else
                {
                    if (Body.mPS.pushedLeftTile)
                    {

                        mDirection = EntityDirection.Right;

                    }
                    else if (Body.mPS.pushedRightTile)
                    {
                        mDirection = EntityDirection.Left;
                    }
                    Body.mSpeed.x = GetMovementSpeed() * (int)mDirection;


                }



                break;
            case EnemyState.Jumping:

                break;
            case EnemyState.Attack1:
                FireRoll();
                break;
            case EnemyState.Attack2:
                TongueWhip();
                break;
        }

        base.EntityUpdate();


    }

    public void FireRoll()
    {
        //CheckForTargets();
        //Renderer.SetAnimState("Attack1");
        //mMovingSpeed = 230;
        Renderer.SetAnimState("Salamander_FireRoll");

        mAttackManager.meleeAttacks[0].Activate();

        Body.mSpeed.x = GetMovementSpeed() * (int)mDirection;

        if (Body.mPS.pushesLeftTile && mDirection == EntityDirection.Left) {
            mDirection = EntityDirection.Right;
            mEnemyState = EnemyState.Moving;
            Renderer.SetAnimState("Salamander_Idle");
        }
        
        if(Body.mPS.pushesRightTile && mDirection == EntityDirection.Right)
        {
            mDirection = EntityDirection.Left;
            mEnemyState = EnemyState.Moving;
            Renderer.SetAnimState("Salamander_Idle");
        }

    }

    public void TongueWhip()
    {
        if (Target != null)
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
            if (!mAttackManager.rangedAttacks[0].mIsActive && !mAttackManager.rangedAttacks[0].OnCooldown())
            {

                Vector2 dir = (Target.Position - Position).normalized;
                mAttackManager.rangedAttacks[0].Activate(dir, Position);

                mEnemyState = EnemyState.Moving;

            }
        }
        else
        {
            mEnemyState = EnemyState.Moving;
        }
    }
}