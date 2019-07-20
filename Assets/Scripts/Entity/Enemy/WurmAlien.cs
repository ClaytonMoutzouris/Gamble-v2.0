using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WurmAlien : Enemy
{

    public Projectile mSlimePrefab;

    public WurmAlien(EnemyPrototype proto) : base(proto)
    {


        Body.mSpeed.x = mMovingSpeed;
        Body.mIsKinematic = false;
        Body.mIsHeavy = false;
        //Body.mAABB.Scale = new Vector3(.5f, .5f, .5f);
        



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

                break;
            case EnemyState.Moving:

                if (Target != null)
                {
                    //Replace this with pathfinding to the target


                    RangedAttack attack = mAttackManager.rangedAttacks[0];

                    attack.Activate((Target.Position - Position).normalized);


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
