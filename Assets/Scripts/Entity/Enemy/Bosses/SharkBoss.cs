using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkBoss : BossEnemy
{
    #region SetInInspector
    public Vector3 attackAnchor;
    public int shotcount = 0;
    public int numShots = 100;
    public int waterSpeed;
    public int baseSpeed;
    #endregion

    public List<Vector2i> shotLocationsAbove = new List<Vector2i>() { new Vector2i(7, 10), new Vector2i(13, 10), new Vector2i(19, 10) };
    public List<Vector2i> shotLocationsWater = new List<Vector2i>() { new Vector2i(7, 3), new Vector2i(13, 3), new Vector2i(19, 3) };

    Vector2 shotLocation;

    public SharkBoss(BossPrototype proto) : base(proto)
    {

        Body.mIsKinematic = true;
        Body.mIsHeavy = true;
        Body.mOffset = Vector2.zero;
        baseSpeed = proto.movementSpeed;
        waterSpeed = baseSpeed * 2;

    }

    public override void EntityUpdate()
    {

        if(Body.mPS.inWater)
        {
            mMovingSpeed = waterSpeed;
        } else
        {
            mMovingSpeed = baseSpeed;

        }


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

                    int random = Random.Range(0, 100);

                    switch(random)
                    {
                        case int n when (n <= 10):
                            mBossState = BossState.Attack2;
                            if(Target.Body.mPS.inWater)
                            {
                                shotLocation = Map.GetMapTilePosition(shotLocationsWater[Random.Range(0, shotLocationsWater.Count)]);

                            } else
                            {
                                shotLocation = Map.GetMapTilePosition(shotLocationsAbove[Random.Range(0, shotLocationsAbove.Count)]);

                            }
                            break;
                        case int n when (n > 10):
                            mBossState = BossState.Attack1;
                            break;
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

        if (EnemyBehaviour.TargetInRange(this, Target, 128))
        {
            if (!mAttackManager.rangedAttacks[0].OnCooldown() && !mAttackManager.rangedAttacks[0].mIsActive)
            {
                RangedAttack attack = mAttackManager.rangedAttacks[0];
                attack.Activate(dir, Position);
                mBossState = BossState.Aggrivated;
            }
        }
        else
        {
            Body.mSpeed = (Target.Position + new Vector2(32, 32) - Position).normalized * GetMovementSpeed();

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
            Body.mSpeed = (shotLocation - Position).normalized * GetMovementSpeed();
            Debug.Log("Shark working his way there " + Vector2.Distance(Position, shotLocation));


        }
        else
        {
            Body.mSpeed = Vector2.zero;

            Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, shotcount*25 + 1).normalized * Vector2.right);
            Debug.Log("Shark trying to do the thing " + shotcount);
            if (mAttackManager.rangedAttacks[1].Activate(dir, Position))
            {
                shotcount++;
            }


        }

        if(shotcount > numShots)
        {
            mBossState = BossState.Aggrivated;
            shotcount = 0;

        }
    }
}
