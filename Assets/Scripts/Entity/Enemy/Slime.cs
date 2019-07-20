using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Slime : Enemy {

    float closeRange = 35f;
    float midRange = 70f;
    float longRange = 140f;


    public Slime(EnemyPrototype proto) : base(proto)
    {
        //Body.mAABB.ScaleX *= -1;

        Body.mIsKinematic = false;

    }

    public override void EntityUpdate()
    {

        base.EntityUpdate();

        if (Hostility == Hostility.Hostile)
        {
            EnemyBehaviour.CheckForTargets(this);
        }

        switch (mEnemyState)
        {
            case EnemyState.Idle:
                Renderer.SetAnimState("Idle");
                break;
            case EnemyState.Moving:

                if(Target != null)
                {
                    //Replace this with pathfinding to the target

                    if (EnemyBehaviour.TargetInRange(this, Target, closeRange))
                    {
                        if (!mAttackManager.meleeAttacks[0].onCooldown)
                        {
                            Renderer.SetAnimState("Slime_Attack");
                            EnemyBehaviour.MeleeAttack(this, 0);
                            EnemyBehaviour.Wait(this, 2, EnemyState.Idle);
                        }
                        //StartCoroutine(EnemyBehaviour.Wait(this, mAttackManager.AttackList[0].duration + 2, EnemyState.Moving));
                    }
                    else
                    {

                        if (Target.Position.x > Position.x)
                        {
                            //If they can't proceed horizontally, try jumping.
                            if (Body.mPS.pushesRightTile && Body.mPS.pushesBottom)
                            {
                                EnemyBehaviour.Jump(this, 460);
                            }
                            //body.mSpeed.x = mMovingSpeed;
                            mDirection = EntityDirection.Right;
                        }
                        else
                        {
                            if (Body.mPS.pushesLeftTile && Body.mPS.pushesBottom)
                            {
                                EnemyBehaviour.Jump(this, 460);
                            }
                            mDirection = EntityDirection.Left;
                        }

                        if (!EnemyBehaviour.TargetInRange(this, Target, midRange)){
                            Renderer.SetAnimState("Slime_Move");
                            EnemyBehaviour.MoveHorizontal(this);                
                        }
                        

                    }

                }
                else
                {
                    if (Body.mPS.pushedLeftTile)
                    {

                        mMovingSpeed = Mathf.Abs(mMovingSpeed);

                    }
                    else if (Body.mPS.pushedRightTile)
                    {
                        mMovingSpeed = -Mathf.Abs(mMovingSpeed);
                    }

                    Body.mSpeed.x = mMovingSpeed;
                    Renderer.SetAnimState("Slime_Move");
                    EnemyBehaviour.MoveHorizontal(this);

                }

                break;
            case EnemyState.Jumping:
                if (Body.mPS.pushesBottom)
                {
                    mEnemyState = EnemyState.Moving;
                    break;
                }

                if (Target != null)
                {
                    //Replace this with pathfinding to the target

                    if (EnemyBehaviour.TargetInRange(this, Target, closeRange))
                    {
                        EnemyBehaviour.MeleeAttack(this, 0);
                    }
                    else
                    {

                        if (Target.Position.x > Position.x)
                        {
                            mDirection = EntityDirection.Right;
                        }
                        else
                        {
                            mDirection = EntityDirection.Left;
                        }

                        EnemyBehaviour.MoveHorizontal(this);
                    }

                }


                break;
            case EnemyState.Attacking:

                bool done = true;

                foreach(Attack attack in mAttackManager.meleeAttacks)
                {
                    if (attack.mIsActive)
                    {
                        Renderer.SetAnimState("Slime_Attack");
                        done = false;
                    }
                }

                if (done)
                {
                    Renderer.SetAnimState("Slime_Move");
                    mEnemyState = EnemyState.Moving;
                }

                break;
        }

        if (Body.mSpeed.x > 0)
        {
            Body.mAABB.ScaleX = 1;
        } else if (Body.mSpeed.x < 0)
        {
            Body.mAABB.ScaleX = -1;

        }

        CollisionManager.UpdateAreas(HurtBox);
        CollisionManager.UpdateAreas(Sight);
        Sight.mEntitiesInSight.Clear();

        
    }

}
