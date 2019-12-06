using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    [SerializeField]
    int lockID;

    public override List<InventoryOption> GetInventoryOptions()
    {
        return new List<InventoryOption>() {
            InventoryOption.Move,
            InventoryOption.Drop,
            InventoryOption.Cancel };
    }
}
