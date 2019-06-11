using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Slime : Enemy {

    float closeRange = 35f;
    float midRange = 70f;
    float longRange = 140f;


    public override void EntityInit()
    {
        base.EntityInit();
        //Body.mAABB.ScaleX *= -1;

        body.mIsKinematic = false;

        EnemyInit();

        StartCoroutine(EnemyBehaviour.Wait(this, 2, EnemyState.Moving));
    }

    public override void EntityUpdate()
    {

        EnemyUpdate();
        base.EntityUpdate();

        if(Hostility == Hostility.Hostile)
        {
            EnemyBehaviour.CheckForTargets(this);
        }

        switch (mEnemyState)
        {
            case EnemyState.Idle:
                mAnimator.Play("Idle");
                break;
            case EnemyState.Moving:

                if(Target != null)
                {
                    //Replace this with pathfinding to the target

                    if (EnemyBehaviour.TargetInRange(this, Target, closeRange))
                    {
                        if (!mAttackManager.AttackList[0].onCooldown)
                        {
                            mAnimator.Play("Slime_Attack");
                            EnemyBehaviour.Attack(this, 0);
                            EnemyBehaviour.Wait(this, 2, EnemyState.Idle);
                        }
                        //StartCoroutine(EnemyBehaviour.Wait(this, mAttackManager.AttackList[0].duration + 2, EnemyState.Moving));
                    }
                    else
                    {

                        if (Target.Position.x > body.mPosition.x)
                        {
                            //If they can't proceed horizontally, try jumping.
                            if (body.mPS.pushesRightTile && body.mPS.pushesBottom)
                            {
                                EnemyBehaviour.Jump(this, 460);
                            }
                            //body.mSpeed.x = mMovingSpeed;
                            mDirection = EntityDirection.Right;
                        }
                        else
                        {
                            if (body.mPS.pushesLeftTile && body.mPS.pushesBottom)
                            {
                                EnemyBehaviour.Jump(this, 460);
                            }
                            mDirection = EntityDirection.Left;
                        }

                        if (!EnemyBehaviour.TargetInRange(this, Target, midRange)){
                            mAnimator.Play("Slime_Move");
                            EnemyBehaviour.MoveHorizontal(this);                
                        }
                        

                    }

                }
                else
                {
                    if (body.mPS.pushedLeftTile)
                    {

                        mDirection = EntityDirection.Right;

                    }
                    else if (body.mPS.pushedRightTile)
                    {
                        mDirection = EntityDirection.Left;
                    }
                    mAnimator.Play("Slime_Move");
                    EnemyBehaviour.MoveHorizontal(this);

                }

                break;
            case EnemyState.Jumping:
                if (body.mPS.pushesBottom)
                {
                    mEnemyState = EnemyState.Moving;
                    break;
                }

                if (Target != null)
                {
                    //Replace this with pathfinding to the target

                    if (EnemyBehaviour.TargetInRange(this, Target, closeRange))
                    {
                        EnemyBehaviour.Attack(this, 0);
                    }
                    else
                    {

                        if (Target.Position.x > body.mPosition.x)
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

                foreach(Attack attack in mAttackManager.AttackList)
                {
                    if (attack.mIsActive)
                    {
                        mAnimator.Play("Slime_Attack");
                        done = false;
                    }
                }

                if (done)
                {
                    mAnimator.Play("Slime_Move");
                    mEnemyState = EnemyState.Moving;
                }

                break;
        }

        if (body.mSpeed.x > 0)
        {
            body.mAABB.ScaleX = 1;
        } else if (body.mSpeed.x < 0)
        {
            body.mAABB.ScaleX = -1;

        }

        CollisionManager.UpdateAreas(HurtBox);
        CollisionManager.UpdateAreas(Sight);
        Sight.mEntitiesInSight.Clear();

        
    }


    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        //CloseRange
        Gizmos.color = new Color(180, 100, 0);
        Gizmos.DrawLine(new Vector3(this.Position.x, this.Position.y + 2, 0), new Vector3(this.Position.x + closeRange,this.Position.y + 2,0));
        //MidRange
        Gizmos.color = new Color(230, 60, 0);
        Gizmos.DrawLine(new Vector3(this.Position.x,this.Position.y + 4, 0), new Vector3(this.Position.x + midRange, this.Position.y + 4, 0));
        //LongRange
        Gizmos.color = new Color(255,0,0);
        Gizmos.DrawLine(new Vector3(this.Position.x, this.Position.y + 6, 0), new Vector3(this.Position.x + longRange, this.Position.y + 6, 0));
        
    }

}
