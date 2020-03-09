using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkBoss : BossEnemy
{
    #region SetInInspector
    public Vector3 attackAnchor;
    public int shotcount = 0;
    public int numShots = 100;
    #endregion

    public Vector2 shotLocation = new Vector2(320, 160);



    public SharkBoss(BossPrototype proto) : base(proto)
    {

        Body.mIsKinematic = true;
        Body.mIsHeavy = true;

    }

    public override void EntityUpdate()
    {




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

                    int random = Random.Range(0, 2);

                    if(random == 0)
                    {
                        mBossState = BossState.Attack1;

                    } else
                    {
                        mBossState = BossState.Attack2;
                        shotcount = 0;
                    }


                }
                else
                {
                   


                }
                break;
            case BossState.Attack1:
                ChaseAndBurstAttack();

                break;
            case BossState.Attack2:
                SuperAttack();
                break;
        }



        base.EntityUpdate();


        //make sure the hitbox follows the object
    }

    public void ChaseAndBurstAttack()
    {
        //Replace this with pathfinding to the target
        Vector2 dir = (Target.Body.mAABB.Center - (Body.mAABB.Center + attackAnchor)).normalized;

        if (EnemyBehaviour.TargetInRange(this, Target, 64))
        {
            if (!mAttackManager.rangedAttacks[0].onCooldown)
            {
                RangedAttack attack = mAttackManager.rangedAttacks[0];
                attack.Activate(dir, Position);
                mBossState = BossState.Aggrivated;
            }
        }
        else
        {
            Body.mSpeed = (Target.Position + new Vector2(32, 32) - Position).normalized * mMovingSpeed;

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

    public void SuperAttack()
    {
        if(Vector2.Distance(Position, shotLocation) > 10)
        {
            Body.mSpeed = (shotLocation - Position).normalized * mMovingSpeed;
            Debug.Log("Shark working his way there " + Vector2.Distance(Position, shotLocation));


        }
        else
        {
            Body.mSpeed = Vector2.zero;

            Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, shotcount*25 + 1) * Vector2.right);
            Debug.Log("Shark trying to do the thing " + shotcount);
            if (mAttackManager.rangedAttacks[1].Activate(dir, Position))
            {
                shotcount++;
            }


        }

        if(shotcount > numShots)
        {
            mBossState = BossState.Aggrivated;
        }
    }
}
