using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonMinibossItem : ConsumableItem
{

    public MinibossType minibossType;


    public override bool Use(Player player)
    {
        foreach (Ability effect in player.abilities)
        {
            effect.OnConsumeItem(player, this);
        }

        Vector2i tilePos = MapManager.instance.GetMapTileAtPoint(player.Position);
        MinibossData data = new MinibossData(tilePos.x, tilePos.y, minibossType);
        Miniboss spawn = MapManager.instance.AddMinibossEntity(data);
        spawn.SetHostility(Hostility.Friendly);

        return true;
        //player.Inventory.RemoveItem(index);
    }

}
