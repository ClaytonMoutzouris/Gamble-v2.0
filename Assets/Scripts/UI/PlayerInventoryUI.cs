using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class PlayerInventoryUI : MonoBehaviour
{
    public List<InventoryNode> items;
    public InventoryNode nodePrefab;
    

    public void AddItem(Item item)
    {
        InventoryNode node = Instantiate(nodePrefab, transform);
        node.SetItem(item);
        items.Add(node);

    }


    
}

