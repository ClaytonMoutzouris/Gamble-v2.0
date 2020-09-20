using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonEnemyItem : ConsumableItem
{

    public EnemyType enemyType;


    public override bool Use(Player player)
    {
        foreach (Ability effect in player.abilities)
        {
            effect.OnConsumeItem(player, this);
        }

        Vector2i tilePos = MapManager.instance.GetMapTileAtPoint(player.Position);
        EnemyData data = new EnemyData(tilePos.x, tilePos.y, enemyType);
        Enemy spawn = MapManager.instance.AddEnemyEntity(data);
        spawn.SetHostility(Hostility.Friendly);

        return true;
        //player.Inventory.RemoveItem(index);
    }

}
