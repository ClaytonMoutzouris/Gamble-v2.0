using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : Enemy
{
    EnemyType spawnType = EnemyType.Slime;
    public List<Enemy> spawns;
    float spawnTimer = 0;
    float spawnCooldown = 5;
    Vector2 spawnOffset = new Vector2(0, 32);

    public Nest(EnemyPrototype proto) : base(proto)
    {
        Body.mIsKinematic = true;
    }

    public override void ActuallyDie()
    {
        base.ActuallyDie();
    }

    public override void Destroy()
    {
        base.Destroy();
    }

    public override void Die()
    {
        base.Die();
    }

    public override void DropLoot()
    {
        base.DropLoot();
    }

    public override void EntityUpdate()
    {
        EnemyBehaviour.CheckForTargets(this);

        if (Target != null)
        {
            spawnTimer += Time.deltaTime;

            if(spawnTimer >= spawnCooldown)
            {
                EnemyPrototype proto = EnemyDatabase.GetEnemyPrototype(spawnType);
                Slime temp = new Slime(proto);
                temp.Spawn(Position + spawnOffset);
                //EnemyBehaviour.Jump(temp, 120);
                spawnTimer = 0;
            }

        }

        base.EntityUpdate();
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override void GetHurt(Attack attack)
    {
        base.GetHurt(attack);
    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();
    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
