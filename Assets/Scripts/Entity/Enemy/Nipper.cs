using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nipper : Enemy
{



    public Nipper(EnemyPrototype proto) : base(proto)
    {

        Body.mIsKinematic = false;
        Body.mIgnoresGravity = true;
        Body.mIgnoresOneWay = true;

        mEnemyState = EnemyState.Moving;
    }

    public override void EntityUpdate()
    {
        //This is just a test, probably dont need to do it this way

        EnemyBehaviour.CheckForTargets(this);

        if(Body.mPS.inWater)
        {
            Body.mIgnoresGravity = true;

        }
        else
        {
            Body.mIgnoresGravity = false;
        }

        switch (mEnemyState)
        {
            case EnemyState.Idle:
                Renderer.SetAnimState("Idle");

                break;
            case EnemyState.Moving:


                if (Target != null)
                {
                    Renderer.SetAnimState("Attack");

                    Vector2 dir = (Target.Body.mAABB.Center - (Body.mAABB.Center)).normalized;

                    if (Body.mPS.inWater)
                    {

                        Body.mSpeed = ((Vector2)Target.Position - Position).normalized * GetMovementSpeed();

                       
                    } else
                    {
                        Body.mSpeed.x = (int)mDirection * GetMovementSpeed();



                    }

                    if (Vector2.Distance(Position, Target.Position) < 64 && Target.Position.y > Position.y)
                    {
                        EnemyBehaviour.Jump(this, prototype.jumpHeight, dir);
                    }
                    //Replace this with pathfinding to the target


                    if (dir.x < 0)
                    {
                        mDirection = EntityDirection.Left;
                    } else
                    {
                        mDirection = EntityDirection.Right;
                    }
                    
                    if (!mAttackManager.meleeAttacks[0].OnCooldown())
                    {
                        MeleeAttack attack = mAttackManager.meleeAttacks[0];
                        attack.Activate();
                    }

                }
                else
                {

                    Renderer.SetAnimState("Idle");

                    if (Body.mPS.pushesLeft && mDirection == EntityDirection.Left)
                    {
                        mDirection = EntityDirection.Right;
                    }
                    else if (Body.mPS.pushesRight && mDirection == EntityDirection.Right)
                    {
                        mDirection = EntityDirection.Left;
                    }


                        Body.mSpeed.x = GetMovementSpeed() * (int)mDirection;



                }

                break;

            case EnemyState.Jumping:

                if(Body.mSpeed.y < 0 && Body.mPS.inWater)
                {
                    mEnemyState = EnemyState.Moving;
                    break;
                }

                if(Body.mPS.pushesBottom)
                {
                    mEnemyState = EnemyState.Moving;
                }

                if(Target != null)
                {
                    Renderer.SetAnimState("Attack");
                    if (!mAttackManager.meleeAttacks[0].OnCooldown())
                    {
                        MeleeAttack attack = mAttackManager.meleeAttacks[0];
                        attack.Activate();
                    }
                } else
                {
                    Renderer.SetAnimState("Idle");

                }

                break;
        }

        base.EntityUpdate();


        //HurtBox.mCollisions.Clear();
        //UpdatePhysics();

        //make sure the hitbox follows the object
    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();

    }

}
