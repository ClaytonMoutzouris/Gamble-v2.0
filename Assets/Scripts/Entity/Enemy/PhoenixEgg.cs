using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoenixEgg : Enemy
{

    public float hatchTimer = 0.0f;
    public float hatchTime = 5.0f;
    bool hatched = false;

    public PhoenixEgg(EnemyPrototype proto) : base(proto)
    {
        hatchTimer = 0;
    }

    public override void Die()
    {
        base.Die();
    }

    public override void EntityUpdate()
    {
        hatchTimer += Time.deltaTime;

        if(hatchTimer >= hatchTime)
        {
            Hatch();
        } else if(hatchTimer >= hatchTime/2)
        {

        }

        base.EntityUpdate();
    }

    public void Hatch()
    {
        hatched = true;
        PhoenixBoss boss = new PhoenixBoss(Resources.Load("Prototypes/Entity/Bosses/PhoenixBoss") as BossPrototype);
        boss.Spawn(Position);
        Die();
    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);
    }
}
