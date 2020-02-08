using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Spawn : EntityEffects
{
    public List<EntityPrototype> spawnObjects;

    public override void OnDeath(Entity entity)
    {
        base.OnDeath(entity);

        foreach (EntityPrototype spawnObject in spawnObjects)
        {
            if(spawnObject is EnemyPrototype enemy)
            {
                if (enemy.enemyType == EnemyType.Ghost)
                {
                    Ghost spawnedEntity = new Ghost(ScriptableObject.Instantiate(enemy));

                    spawnedEntity.Spawn(entity.Position + new Vector2(Random.Range(-32, 32), Random.Range(-32, 32)));
                }
            }

        }
       
    }

}
