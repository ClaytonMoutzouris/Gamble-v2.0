using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBoss : BossEnemy
{
    #region SetInInspector



    #endregion




    public CatBoss(BossPrototype proto) :base(proto)
    {
        Body.mIsKinematic = true;
        Body.mIsHeavy = true;
    }
    
    public override void EntityUpdate()
    {

        base.EntityUpdate();



        switch (mBossState)
        {
            case BossState.Idle:

                CheckForTargets();

                break;
            case BossState.Aggrivated:
                CheckForTargets();

                if (Target != null)
                {
                    //Replace this with pathfinding to the target

                    if (!mAttackManager.rangedAttacks[0].onCooldown)
                    {
                        Vector2 dir = ((Vector2)Target.Position - Position).normalized;
                        RangedAttack attack = mAttackManager.rangedAttacks[0];
                        attack.Activate(dir);
                    }


                    if (Target.Position.x > Position.x)
                    {
                        if (Body.mPS.pushesRightTile && Body.mPS.pushesBottom)
                        {
                            Body.mSpeed.y = 460;

                        }
                        mMovingSpeed = Mathf.Abs(mMovingSpeed);
                    }
                    else
                    {
                        if (Body.mPS.pushesLeftTile && Body.mPS.pushesBottom)
                        {
                            Body.mSpeed.y = 460;
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
        //UpdatePhysics();

        //make sure the hitbox follows the object
    }
}
