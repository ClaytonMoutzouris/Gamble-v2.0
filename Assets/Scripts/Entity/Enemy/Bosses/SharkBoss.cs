using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkBoss : BossEnemy
{
    #region SetInInspector
    public Vector3 attackAnchor;

    #endregion




    public SharkBoss(BossPrototype proto) : base(proto)
    {

        Body.mIsKinematic = true;
        Body.mIsHeavy = true;

    }

    public override void EntityUpdate()
    {

        mAttackManager.UpdateAttacks();
        base.EntityUpdate();



        switch (mBossState)
        {
            case BossState.Idle:

                CheckForTargets();

                break;
            case BossState.Aggrivated:
                CheckForTargets();
                //Debug.Log("Got here 1");
                if (Target != null)
                {
                    //Replace this with pathfinding to the target
                    Vector2 dir = (Target.Body.mAABB.Center - (Body.mAABB.Center + attackAnchor)).normalized;

                    if (!mAttackManager.rangedAttacks[0].onCooldown)
                    {
                        RangedAttack attack = mAttackManager.rangedAttacks[0];
                        attack.Activate(dir);
                    }

                    if (dir.x < 0)
                    {
                        mDirection = EntityDirection.Right;
                        //Body.mAABB.ScaleX = 1;
                    }
                    else
                    {
                        mDirection = EntityDirection.Left;
                        //Body.mAABB.ScaleX = -1;

                    }

                }
                else
                {
                   


                }

                Body.mSpeed.x = mMovingSpeed;

                break;
            }


        CollisionManager.UpdateAreas(HurtBox);

        CollisionManager.UpdateAreas(Sight);
        Sight.mEntitiesInSight.Clear();

        //make sure the hitbox follows the object
    }


}
