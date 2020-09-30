using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoenixBoss : BossEnemy
{
    #region SetInInspector
    public Vector3 attackAnchor;
    public int shotcount = 0;
    public int numShots = 5;
    public int flamethrowerShots = 10;
    int randomXoffset = 0;
    #endregion

    public List<Vector2i> shotLocations = new List<Vector2i>() { new Vector2i(7, 6), new Vector2i(13, 7), new Vector2i(19, 6) };
    Vector2 shotLocation;

    public PhoenixBoss(BossPrototype proto) : base(proto)
    {

        Body.mIsKinematic = true;

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

                    int random = Random.Range(0, 100);
                    switch (random)
                    {
                        case int n when (n <= 10):
                            mBossState = BossState.Attack1;
                            shotLocation = Map.GetMapTilePosition(shotLocations[Random.Range(0, shotLocations.Count)]);

                            break;
                        case int n when (n > 10 && n <= 50):
                            mBossState = BossState.Attack2;
                            break;
                        case int n when (n > 50):
                            mBossState = BossState.Attack3;
                            randomXoffset = Random.Range(-64, 64);
                            shotLocation = Target.Position + new Vector2(randomXoffset, 64);
                            break;
                    }



                }
                else
                {



                }
                break;
            case BossState.Attack1:
                SuperAttack();

                break;
            case BossState.Attack2:
                ChaseAndBurstAttack();

                break;
            case BossState.Attack3:
                Flamethrower();
                break;
        }



        base.EntityUpdate();


        //make sure the hitbox follows the object
    }

    public void ChaseAndBurstAttack()
    {
        //Replace this with pathfinding to the target
        Vector2 dir = (Target.Body.mAABB.Center - (Body.mAABB.Center + attackAnchor)).normalized;

        if (EnemyBehaviour.TargetInRange(this, Target, 124))
        {
            if (!mAttackManager.rangedAttacks[0].OnCooldown())
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

    public void Flamethrower()
    {
        Vector2 dir = (Target.Body.mAABB.Center - (Body.mAABB.Center + attackAnchor)).normalized;


        if (Vector2.Distance(Position, shotLocation) < 10)
        {
            shotLocation = Position;
            Body.mSpeed = Vector2.zero;

            if (mAttackManager.rangedAttacks[2].Activate(dir, Position))
            {
                shotcount++;
            
            }
        }
        else
        {
            shotLocation = Target.Position + new Vector2(randomXoffset, 64);
            Body.mSpeed = (shotLocation - Position).normalized * GetMovementSpeed();

        }

        if (shotcount > flamethrowerShots)
        {
            mBossState = BossState.Aggrivated;
            shotcount = 0;

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
        if (Vector2.Distance(Position, shotLocation) > 10)
        {
            Body.mSpeed = (shotLocation - Position).normalized * (GetMovementSpeed() + 50);
            Debug.Log("Shark working his way there " + shotLocation);


        }
        else
        {
            Body.mSpeed = Vector2.zero;

            Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, shotcount * 45 + 1) * Vector2.right);
            Debug.Log("Shark trying to do the thing " + shotcount);
            if (mAttackManager.rangedAttacks[1].Activate(dir, Position))
            {
                shotcount++;
            }


        }

        if (shotcount > numShots)
        {
            mBossState = BossState.Aggrivated;
            shotcount = 0;

        }
    }

    public override void Die()
    {
        MapManager.instance.HardenLava();
        PhoenixEgg boss = new PhoenixEgg(Resources.Load("Prototypes/Entity/Enemies/PhoenixEgg") as EnemyPrototype);
        boss.Spawn(Position);
        base.Die();
    }
}
