using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState { Idle, Aggrivated, Attack1, Attack2, Attack3, Attack4, Attack5 };

public class BossEnemy : Enemy
{
    public BossState mBossState = BossState.Idle;
    public BossType bossType = BossType.Count;
    public List<int> phaseTimers;

    public List<Projectile> projectilePrefabs;
    public List<Action> bossActions;

    public bool bossTrigger = false;

    public BossEnemy(BossPrototype proto) : base(proto)
    {
        bossType = proto.bossType;
        ExpValue = 100;
    }

    public void CheckForTargets()
    {
        this.Target = null;
        if (Sight.mEntitiesInSight != null)
        {
            foreach (Entity entity in Sight.mEntitiesInSight)
            {
                if (entity is Player)
                {
                    if(!((Player)entity).IsDead)
                    {
                        this.Target = (Player)entity;
                        TriggerBoss();
                        break;
                    }
                }
            }
        }
    }

    public void TriggerBoss()
    {
        if (bossTrigger)
            return;

        mBossState = BossState.Aggrivated;
        bossTrigger = true;
        SoundManager.instance.PlayBossMusic((int)bossType);
    }

    protected virtual void Idle()
    {
        CheckForTargets();
    }

    public override void DropLoot()
    {
 
        foreach (Item item in prototype.lootTable.GetLoot(WorldManager.instance.NumCompletedWorlds()))
        {
            ItemObject temp = new ItemObject(ItemDatabase.NewItem(item), Resources.Load("Prototypes/Entity/Objects/ItemObject") as EntityPrototype);
            temp.Spawn(Position + new Vector2(0, MapManager.cTileSize / 2));
        }
        


    }

    public override void Die()
    {
        SoundManager.instance.PlayLevelMusic((int)MapManager.instance.mCurrentMap.worldType);

        WorldManager.instance.WorldCleared(WorldManager.instance.currentWorldIndex);
        
        base.Die();
    }
}
