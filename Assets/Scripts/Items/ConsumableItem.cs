using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{
    public bool isHealingItem;
    public virtual bool Use(Player player)
    {
        foreach(Ability effect in player.abilities)
        {
            effect.OnConsumeItem(player, this);
        }

        return true;
        //player.Inventory.RemoveItem(index);
    }

    public override List<InventoryOption> GetInventoryOptions()
    {
        return new List<InventoryOption>() {
            InventoryOption.Use,
            InventoryOption.Move,
            InventoryOption.Drop,
            InventoryOption.Cancel };
    }
}