using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReusableMedkits : Effect
{


    public ReusableMedkits()
    {
        effectName = "ReusableMedkits";
        type = EffectType.ReusableMedkits;
    }

    public override void OnConsumeItem(Player player, ConsumableItem item)
    {
        base.OnConsumeItem(player, item);

        if (item is Medkit)
        {
            int r = Random.Range(0, 100);

            if(r > 50)
            {
                player.Inventory.AddItemToInventory(ItemDatabase.NewItem(item));
            }
        }

    }
}
