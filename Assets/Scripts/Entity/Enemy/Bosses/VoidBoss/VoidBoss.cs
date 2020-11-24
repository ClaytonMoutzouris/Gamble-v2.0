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
    public EnemyPart leftHand;
    public EnemyPart rightHand;

    public Vector2 startPos;
    public Vector2 endPos;
    public bool attackBegin;
    public Vector2 mapCenter = new Vector2(12*32, 12*32);
    public Vector2 swipeOffset = new Vector2(0, 5 * 32);
    public bool leftHandFollow = true;
    public bool rightHandFollow = true;

    public Vector2 leftHandDestination;
    public Vector2 rightHandDestination;

    public float voidlingSpawnRate = 1;
    public float voidlingSpawnDuration = 5;
    public float voidlingTimestamp = 0;
    public float spawnTimestamp = 0;

    public bool enraged = false;

    public VoidBoss(BossPrototype proto) : base(proto)
    {
        Body.mAABB.Offset += (Vector3)proto.bodyOffset;
        HurtBox.mAABB.Offset += (Vector3)proto.bodyOffset;
        Body.mIsKinematic = true;
        abilityFlags.SetFlag(AbilityFlag.Heavy, true);

        leftHand = new EnemyPart(Resources.Load("Prototypes/Entity/Enemies/VoidBossLeftHand") as EnemyPrototype, this);
        leftHand.offset = new Vector2(175, -95);
        rightHand = new EnemyPart(Resources.Load("Prototypes/Entity/Enemies/VoidBossRightHand") as EnemyPrototype, this);
        rightHand.offset = new Vector2(-175, -95);

    }

    public override void TriggerBoss()
    {
        if (bossTrigger)
        {
            return;
        }

        base.TriggerBoss();

    }

    public void Enrage()
    {
        if(enraged)
        {
            return;
        }

        enraged = true;

        voidlingSpawnRate = 2;
        mMovingSpeed = mMovingSpeed * 2;
        mStats.AddBonus(new StatBonus(StatType.Attack, 20));
        Renderer.Sprite.color = Color.red;
    }

    public override void EntityUpdate()
    {
        if(mHealth.currentHealth <= mHealth.maxHealth/2)
        {
            Enrage();
        }
        switch (mBossState)
        {
            case BossState.Idle:

                CheckForTargets();

                break;
            case BossState.Aggrivated:
                CheckForTargets();

                if (Vector2.Distance(Position, mapCenter) > 32)
                {
                    Body.mSpeed = (mapCenter - Position).normalized * GetMovementSpeed()*2;
                    break;
                }

                int random = Random.Range(0, 3);

                switch (random)
                {
                    case 0:
                        //Eye laser
                        int randomLevel = Random.Range(1, 4);

                        startPos = new Vector2(0, randomLevel * 7 * 32);
                        endPos = new Vector2(25*32, randomLevel * 7 * 32);
                        attackBegin = false;
                        mBossState = BossState.Attack1;
                        break;
                        
                    case 1:
                        attackBegin = false;
                        mBossState = BossState.Attack2;
                        break;
                    case 2:
                        mBossState = BossState.Attack3;
                        voidlingTimestamp = Time.time;

                        break;
                }

                break;
            case BossState.Attack1:
                VoidLaser();

                break;
            case BossState.Attack2:
                VoidSwipe();

                break;
            case BossState.Attack3:
                SpawnVoidlings();
                break;
        }



        base.EntityUpdate();

        if(leftHandFollow)
        {
            leftHand.Position = Vector2.Lerp(leftHand.Position, Position + leftHand.offset, leftHand.mMovingSpeed / 10);

        }

        if (rightHandFollow)
        {
            rightHand.Position = Vector2.Lerp(rightHand.Position, Position + rightHand.offset, rightHand.mMovingSpeed / 10);
        }

        //make sure the hitbox follows the object
    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);

        leftHand.Spawn(Position + leftHand.offset);
        rightHand.Spawn(Position + rightHand.offset);

    }

    public void VoidLaser()
    {
        if(!attackBegin)
        {
            if (Vector2.Distance(Position, startPos) < 32)
            {
                attackBegin = true;
            }
            else
            {
                Body.mSpeed = (startPos - Position).normalized * GetMovementSpeed()*2;
            }
        }
        else
        {
            mAttackManager.meleeAttacks[0].Activate();
            if (Vector2.Distance(Position, endPos) < 32)
            {
                mBossState = BossState.Aggrivated;
                mAttackManager.meleeAttacks[0].Deactivate();

            }
            else
            {
                Body.mSpeed = (endPos - Position).normalized * GetMovementSpeed();
            }
        }
        
    }

    public void VoidSwipe()
    {
        CheckForTargets();
        if(Target == null)
        {
            mBossState = BossState.Aggrivated;
            leftHand.mAttackManager.meleeAttacks[0].Deactivate();
            rightHand.mAttackManager.meleeAttacks[0].Deactivate();
            leftHandFollow = true;
            rightHandFollow = true;
            return;
        }

        if (!attackBegin)
        {
            if (Vector2.Distance(Position, Target.Position + swipeOffset) < 32)
            {
                attackBegin = true;

                Vector2 leftHandDir = (Target.Position - leftHand.Position).normalized;
                leftHandDestination = Target.Position + 64 * leftHandDir;


                Vector2 rightHandDir = (Target.Position - rightHand.Position).normalized;
                rightHandDestination = Target.Position + 64 * rightHandDir;

                Body.mSpeed = Vector2.zero;
            }
            else
            {
                Body.mSpeed = (Target.Position + swipeOffset - Position).normalized * GetMovementSpeed();
            }
        } else
        {
            leftHandFollow = false;
            rightHandFollow = false;

            leftHand.mAttackManager.meleeAttacks[0].Activate();
            rightHand.mAttackManager.meleeAttacks[0].Activate();

            if (Vector2.Distance(rightHandDestination, rightHand.Position) < 32)
            {
                rightHandFollow = true;
            }
            else
            {
                Vector2 rightHandDir = (rightHandDestination - rightHand.Position).normalized;
                rightHand.Body.mSpeed = rightHandDir * rightHand.GetMovementSpeed();
            }

            if (Vector2.Distance(leftHandDestination, leftHand.Position) < 32)
            {
                leftHandFollow = true;
            } else
            {

                Vector2 leftHandDir = (leftHandDestination - leftHand.Position).normalized;
                leftHand.Body.mSpeed = leftHandDir * leftHand.GetMovementSpeed();
            }


            if(rightHandFollow || leftHandFollow)
            {
                mBossState = BossState.Aggrivated;
                leftHand.mAttackManager.meleeAttacks[0].Deactivate();
                rightHand.mAttackManager.meleeAttacks[0].Deactivate();
                leftHandFollow = true;
                rightHandFollow = true;
            }


        }

    }

    public void SpawnVoidlings()
    {

        if (voidlingTimestamp + voidlingSpawnDuration < Time.time)
        {
            //We are done
            mBossState = BossState.Aggrivated;
            return;
        }

        Body.mSpeed = Vector2.zero;

        Position = mapCenter + Vector2.right * Mathf.Sin((Time.time- voidlingTimestamp) * 1f) * 320 + Vector2.up * (Mathf.Cos((Time.time - voidlingTimestamp) * 4f) * 120-60);

        if(spawnTimestamp + (1/voidlingSpawnRate) < Time.time)
        {
            //spawn a voidling
            Vector2i tilePos = MapManager.instance.GetMapTileAtPoint(Body.mAABB.Center);
            Enemy temp = MapManager.instance.AddEnemyEntity(new EnemyData(tilePos.x, tilePos.y, EnemyType.Voidling));
            spawnTimestamp = Time.time;
        }






    }
}
