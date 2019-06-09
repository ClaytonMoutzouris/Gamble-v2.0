using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : Item
{
    public virtual void Use()
    {
        
    }

    public override List<InventoryOption> GetInventoryOptions()
    {
        return new List<InventoryOption>() {
            InventoryOption.Move,
            InventoryOption.Drop,
            InventoryOption.Cancel };
    }
}
