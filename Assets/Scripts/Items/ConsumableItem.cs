using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{

    public virtual void Use(Player player, int index)
    {
        Debug.Log("User Item:" + index);
        player.mInventory.RemoveItem(index);
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