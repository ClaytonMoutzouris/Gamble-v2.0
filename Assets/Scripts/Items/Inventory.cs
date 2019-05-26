using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Entity mEntity;
    public List<Item> items;

    private void Start()
    {
        mEntity = GetComponent<Entity>();
        items = new List<Item>();
    }

    public void AddItemToInventory(Item item)
    {
        items.Add(item);
    }

    public void AddItemsToInventory(List<Item> items)
    {
        this.items.AddRange(items);
    }

    public Item GetItemAtIndex(int index)
    {
        return items[index];
    }

}
