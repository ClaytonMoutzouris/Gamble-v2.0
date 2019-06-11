using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : QuestItem
{
    [SerializeField]
    int lockID;

    public override void Use()
    {

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
