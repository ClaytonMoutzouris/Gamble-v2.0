using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shroombo : Miniboss
{

    public float waitTime = 2.0f;
    public float waitDuration = 0.0f;

    public Shroombo(EnemyPrototype proto) : base(proto)
    {
        Body.mIsKinematic = true;

    }

    public override void EntityUpdate()
    {

        EnemyBehaviour.CheckForTargets(this);


        switch (mEnemyState)
        {
            case EnemyState.Idle:
                waitDuration += Time.deltaTime;
                if(waitDuration >= waitTime)
                {
                    mEnemyState = EnemyState.Moving;
                }

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

                    int xVariance = Random.Range(0, 100);
                    Body.mSpeed.x = GetMovementSpeed()*(int)mDirection * xVariance/100;
                    EnemyBehaviour.Jump(this, jumpHeight);
                    

                }
                else
                {
                    mEnemyState = EnemyState.Idle;

                }



                break;
            case EnemyState.Jumping:

                float randomX = Random.Range(-1.0f, 1.0f);
                mAttackManager.rangedAttacks[0].Activate(new Vector2(randomX, 1), Position);

                if(Body.mPS.pushesBottom && !Body.mPS.pushedBottom)
                {
                    mEnemyState = EnemyState.Idle;
                    waitDuration = 0.0f;
                    mAttackManager.meleeAttacks[0].Activate();
                    Body.mSpeed.x = 0;

                }
                break;
        }

        base.EntityUpdate();


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
