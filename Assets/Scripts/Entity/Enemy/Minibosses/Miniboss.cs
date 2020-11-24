using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miniboss : Enemy
{
    MinibossType minibossType;

    public bool bossTrigger = false;


    public Miniboss(EnemyPrototype proto) : base(proto)
    {

        ExpValue = 50;
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
                    if (!((Player)entity).IsDead)
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

        bossTrigger = true;
    }

    protected virtual void Idle()
    {
        CheckForTargets();
    }

    public override void DropLoot()
    {

        foreach (Item item in prototype.lootTable.GetLoot())
        {
            ItemObject temp = new ItemObject(ItemDatabase.NewItem(item), Resources.Load("Prototypes/Entity/Objects/ItemObject") as EntityPrototype);
            temp.Spawn(Position + new Vector2(0, MapManager.cTileSize / 2));
        }



    }

    public override void Die()
    {

        base.Die();
    }
}
