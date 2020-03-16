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
    #endregion

    public Vector2 shotLocation = new Vector2(320, 160);



    public VoidBoss(BossPrototype proto) : base(proto)
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
                int random = Random.Range(0, 2);

                if (random == 0)
                {
                    mBossState = BossState.Attack1;

                }
                else
                {
                    mBossState = BossState.Attack2;
                }

                break;
            case BossState.Attack1:
                SummonBlackholes();
                break;
            case BossState.Attack2:
                TeleportAttack();
                break;
        }



        base.EntityUpdate();


        //make sure the hitbox follows the object
    }

    void SummonBlackholes()
    {
        if (Vector2.Distance(Position, shotLocation) > 10)
        {
            Body.mSpeed = (shotLocation - Position).normalized * GetMovementSpeed();

        }
        else
        {
            Body.mSpeed = Vector2.zero;

            Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, shotcount * 25 + 1) * Vector2.right);
            if (mAttackManager.rangedAttacks[0].Activate(dir, new Vector2(Random.Range(0, 20*32), Random.Range(0, 20 * 32))))
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
    
}
