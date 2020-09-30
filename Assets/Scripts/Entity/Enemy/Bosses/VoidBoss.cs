using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidBoss : BossEnemy
{
    #region SetInInspector
    public Vector3 attackAnchor;
    public int shotcount = 0;
    public int numShots = 20;
    public int teleportCount = 0;
    public int numTeleports = 3;
    public bool teleported = false;
    List<Projectile> shieldList = new List<Projectile>(); 
    #endregion

    public bool pathSet = false;

    public VoidBoss(BossPrototype proto) : base(proto)
    {

        Body.mIsKinematic = true;
        abilityFlags.SetFlag(AbilityFlag.Heavy, true);

    }

    public override void TriggerBoss()
    {
        if (bossTrigger)
        {
            return;
        }

        base.TriggerBoss();
        /*
        Vector2 baseOffset = Vector2.up * 160;
        for(int i = 0; i < 10; i++)
        {
            Vector2 tempDir = Vector2.up;
            if (i > 1)
            {
                tempDir = tempDir.Rotate(-360 / 2 + (36 * i));
            }

            tempDir.Normalize();

            Projectile projectile = new Projectile(mAttackManager.rangedAttacks[0].projProto, mAttackManager.meleeAttacks[1], tempDir);
            projectile.Spawn(Position+new Vector2(0, 160) + (160 * tempDir.normalized));
        }
        */

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
                int random = Random.Range(0, 3);


                switch (random)
                {
                    case 0:
                        mBossState = BossState.Attack1;
                        break;
                        
                    case 1:
                        mBossState = BossState.Attack2;
                        break;
                    case 2:
                        mBossState = BossState.Attack3;
                        break;
                }

                break;
            case BossState.Attack1:
                SummonBlackholes();
                break;
            case BossState.Attack2:
                TeleportAttack();
                break;
            case BossState.Attack3:
                SpiralAttack();
                break;
        }



        base.EntityUpdate();


        //make sure the hitbox follows the object
    }

    void SummonBlackholes()
    {

        Body.mSpeed = Vector2.zero;

        Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, shotcount * 25 + 1) * Vector2.right);
        if (mAttackManager.rangedAttacks[0].Activate(dir, new Vector2(Random.Range(0, 20*32), Random.Range(0, 20 * 32))))
        {
            shotcount++;
        }


        if (shotcount > numShots)
        {
            mBossState = BossState.Aggrivated;
            shotcount = 0;
        }
    }

    void TeleportAttack()
    {

        if(!teleported)
        {
            Body.SetTilePosition(new Vector2i(Random.Range(3, 17), Random.Range(3, 17)));
            mAttackManager.rangedAttacks[1].Activate(Vector2.left, Position);
            teleported = true;
            teleportCount++;
        } else
        {
            if(!mAttackManager.rangedAttacks[1].mIsActive)
            {
                teleported = false;
            }
        }

        if(teleportCount >= numTeleports && !mAttackManager.rangedAttacks[1].mIsActive)
        {
            mBossState = BossState.Aggrivated;
            teleported = false;
            teleportCount = 0;
        }
    }

    void SpiralAttack()
    {

        if(pathSet)
        {
            Vector2i mapSize = MapManager.instance.mCurrentMap.getMapSize();
            if (Position.x < 0 || Position.x > mapSize.x * Constants.cTileResolution
                || Position.y < 0 || Position.y > mapSize.y * Constants.cTileResolution)
            {
                mBossState = BossState.Aggrivated;
                Body.SetTilePosition(new Vector2i(mapSize.x/2, mapSize.y/2));
                Body.mSpeed = Vector2.zero;
                pathSet = false;
                mAttackManager.meleeAttacks[0].Deactivate();

            }
        } else
        {
            Body.mSpeed = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * GetMovementSpeed()*5;
            pathSet = true;
            mAttackManager.meleeAttacks[0].Activate();
        }
        





    }

}
