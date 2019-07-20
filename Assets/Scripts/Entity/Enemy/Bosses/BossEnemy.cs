using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState { Idle, Aggrivated, Attack1, Attack2, Attack3 };

public class BossEnemy : Enemy
{
    #region SetInInspector
    public BossState mBossState = BossState.Idle;
    public BossType bossType = BossType.Count;

    public float AttackTimer = 4.0f;
    public List<Projectile> projectilePrefabs;
    #endregion


    protected float AttackCooldown = 0.0f;
    public bool bossTrigger = false;

    public BossEnemy(BossPrototype proto) : base(proto)
    {
        bossType = proto.bossType;

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
                    this.Target = entity;
                    TriggerBoss();
                    break;
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

    public override void Die()
    {
        SoundManager.instance.PlayLevelMusic((int)LevelManager.instance.mMap.mCurrentMap.type);

        base.Die();
    }
}
