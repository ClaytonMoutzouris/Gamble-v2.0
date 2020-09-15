using System.Collections.Generic;
using UnityEngine;

public class TentacleBoss : BossEnemy
{
    #region SetInInspector

    public float mSummonCooldown = 3;
    public float mSummonTimer = 0;
    public int mMaxMinions = 6;
    public List<Enemy> mMinions = new List<Enemy>();
    public int numBeamPillars = 5;
    public int shotcount = 0;
    #endregion




    public TentacleBoss(BossPrototype proto) : base(proto)
    {
        Body.mIsKinematic = true;
        abilityFlags.SetFlag(AbilityFlag.Heavy, true);
        Body.mIgnoresGravity = true;

    }

    public override void EntityUpdate()
    {

        mSummonTimer += Time.deltaTime;
        CheckForTargets();


        switch (mBossState)
        {
            case BossState.Idle:


                break;
            case BossState.Aggrivated:

                int random = Random.Range(0, 100);
                switch (random)
                {
                    case int n when (n <= 50):
                        mBossState = BossState.Attack1;

                        break;
                    case int n when (n > 50 && n <= 80):
                        mBossState = BossState.Attack2;
                        break;
                    case int n when (n > 80):
                        mBossState = BossState.Attack3;
                        shotcount = 0;
                        break;
                }

                if (Target != null)
                {



                    Vector2 dir = ((Vector2)Target.Position - Position).normalized;

                    if (!mAttackManager.rangedAttacks[0].OnCooldown())
                    {
                        RangedAttack attack = mAttackManager.rangedAttacks[0];
                        attack.Activate(dir, Position);
                    }

                    Body.mSpeed = dir * GetMovementSpeed();

                }

                break;
            case BossState.Attack1:

                ShootLaser();
                break;
            case BossState.Attack2:

                SummonEyebat();
                break;
            case BossState.Attack3:

                BeamPillar();
                break;
        }

        if (Body.mSpeed.x > 0)
        {
            mDirection = EntityDirection.Right;
            //Body.mAABB.ScaleX = 1;
        }
        else
        {
            mDirection = EntityDirection.Left;
            //Body.mAABB.ScaleX = -1;

        }

        base.EntityUpdate();



        //HurtBox.mCollisions.Clear();
        //UpdatePhysics();

        //make sure the hitbox follows the object
    }


    public void ShootLaser()
    {
        if (Target != null)
        {
            //Replace this with pathfinding to the target
            Vector2 dir = ((Vector2)Target.Position - Position).normalized;

            if (mAttackManager.rangedAttacks[0].Activate(dir, Position))
            {
                mBossState = BossState.Aggrivated;
            }

        }
    }

    public void BeamPillar()
    {

        if (Target != null)
        {
            int randomX = Random.Range(3, 22);
            if (mAttackManager.rangedAttacks[1].Activate(Vector2.up, Map.GetMapTilePosition(randomX, 5)))
            {
                shotcount++;

            }

        }

        if (shotcount > numBeamPillars)
        {
            mBossState = BossState.Aggrivated;
            shotcount = 0;

        }
    }

    public void SummonEyebat()
    {
        if (Target != null)
        {
            //Replace this with pathfinding to the target
            Vector2 dir = ((Vector2)Target.Position - Position).normalized;
            Body.mSpeed = dir * GetMovementSpeed();

            if (mSummonTimer >= mSummonCooldown)
            {
                Vector2i tilePos = MapManager.instance.GetMapTileAtPoint(Body.mAABB.Center);
                Enemy temp = MapManager.instance.AddEnemyEntity(new EnemyData(tilePos.x, tilePos.y, EnemyType.Eye));
                temp.mEnemyState = EnemyState.Moving;
                mSummonTimer = 0;
                mBossState = BossState.Aggrivated;
            }


        } else
        {
            mBossState = BossState.Aggrivated;

        }

    }

}
