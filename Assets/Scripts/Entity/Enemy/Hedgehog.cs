using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hedgehog : Enemy
{
    public Hedgehog(EnemyPrototype proto) : base(proto)
    {

        Body.mSpeed.x = mMovingSpeed;
        Body.mIsKinematic = false;
        Body.mIsHeavy = false;
        //Body.mAABB.Scale = new Vector3(.5f, .5f, .5f);


        //StartCoroutine(EnemyBehaviour.Wait(this, 2, EnemyState.Moving));

    }

    public override void EntityUpdate()
    {

        base.EntityUpdate();

        if (Hostility == Hostility.Hostile)
        {
            EnemyBehaviour.CheckForTargets(this);
        }

        if(!mAttackManager.meleeAttacks[0].mIsActive)
        {
            Renderer.SetAnimState("Idle");
        }

        switch (mEnemyState)
        {
            case EnemyState.Idle:

                break;
            case EnemyState.Moving:

                if (Target != null)
                {
                    //Replace this with pathfinding to the target

                    if (EnemyBehaviour.TargetInRange(this, Target, Body.mAABB.HalfSizeX + 20))
                    {
                        mAttackManager.meleeAttacks[0].Activate();
                        Renderer.SetAnimState("Attack1");
                    }
                    else
                    {

                        if (Target.Position.x > Position.x)
                        {
                            if (Body.mPS.pushesRightTile && Body.mPS.pushesBottom)
                            {
                                EnemyBehaviour.Jump(this, 460);
                            }
                            mMovingSpeed = Mathf.Abs(mMovingSpeed);
                        }
                        else
                        {
                            if (Body.mPS.pushesLeftTile && Body.mPS.pushesBottom)
                            {
                                EnemyBehaviour.Jump(this, 460);
                            }
                            mMovingSpeed = -Mathf.Abs(mMovingSpeed);
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


                }

                Body.mSpeed.x = mMovingSpeed;

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

                    if (EnemyBehaviour.TargetInRange(this, Target, 20))
                    {
                        mAttackManager.meleeAttacks[0].Activate();
                    }
                    else
                    {

                        if (Target.Position.x > Position.x)
                        {
                            mMovingSpeed = Mathf.Abs(mMovingSpeed);
                        }
                        else
                        {
                            mMovingSpeed = -Mathf.Abs(mMovingSpeed);
                        }
                    }

                }

                Body.mSpeed.x = mMovingSpeed;


                break;
        }

        if (Body.mSpeed.x > 0)
        {
            Body.mAABB.ScaleX = 1;
        }
        else
        {
            Body.mAABB.ScaleX = -1;

        }

        CollisionManager.UpdateAreas(HurtBox);
        CollisionManager.UpdateAreas(Sight);
        Sight.mEntitiesInSight.Clear();

        //HurtBox.mCollisions.Clear();

        //make sure the hitbox follows the object
    }
}
