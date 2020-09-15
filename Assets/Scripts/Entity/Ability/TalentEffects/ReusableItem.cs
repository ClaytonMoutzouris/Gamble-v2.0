using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReusableItem : Ability
{
    public int procChance = 100;
    public ConsumableItem itemType;

    public override void OnConsumeItem(Player player, ConsumableItem item)
    {
        base.OnConsumeItem(player, item);

        if (item.itemName.Equals(itemType.itemName))
        {
            int r = Random.Range(0, 100);

            if(r > procChance)
            {
                player.Inventory.AddItemToInventory(ItemDatabase.NewItem(item));
            }
        }

    }
}
