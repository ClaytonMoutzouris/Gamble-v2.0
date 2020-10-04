using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voidbeast : Miniboss
{

    public bool teleported = false;
    public Voidbeast(EnemyPrototype proto) : base(proto)
    {
        Body.mIsKinematic = true;

    }

    public override void EntityUpdate()
    {

        EnemyBehaviour.CheckForTargets(this);

        if((mHealth.currentHealth/mHealth.maxHealth*100) < 50)
        {
            mAttackManager.meleeAttacks[0].coolDown = 2;
        }

        switch (mEnemyState)
        {
            case EnemyState.Idle:

                break;
            case EnemyState.Moving:


                if (Target != null)
                {
                    Vector2 positionVector = Target.Position - Position;

                    if (positionVector.x > 0)
                    {

                        mDirection = EntityDirection.Right;
                    }
                    else
                    {

                        mDirection = EntityDirection.Left;
                    }

                    if(!mAttackManager.meleeAttacks[0].OnCooldown() && !mAttackManager.meleeAttacks[0].mIsActive)
                    {
                        mEnemyState = EnemyState.Attack1;
                        teleported = false;
                    }

                }



                break;
            case EnemyState.Jumping:

                break;
            case EnemyState.Attack1:
                VoidTails();
                break;
        }

        base.EntityUpdate();


    }

    public void VoidTails()
    {
        if (Target != null)
        {
            if(!teleported)
            {
                Position = Target.Position + 80 * Vector2.left * (int)Target.mDirection;
                Vector2 positionVector = Target.Position - Position;

                if (positionVector.x > 0)
                {

                    mDirection = EntityDirection.Right;
                }
                else
                {

                    mDirection = EntityDirection.Left;
                }
                mAttackManager.meleeAttacks[0].Activate();
                teleported = true;
                Renderer.SetAnimState("Voidbeast_Attack");

            } else
            {
                if(!mAttackManager.meleeAttacks[0].mIsActive)
                {
                    mEnemyState = EnemyState.Moving;
                    Renderer.SetAnimState("Voidbeast_Idle");

                }
            }



        }
        else
        {
            mEnemyState = EnemyState.Moving;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();
    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);
    }

    protected override void Idle()
    {
        base.Idle();
    }
}
