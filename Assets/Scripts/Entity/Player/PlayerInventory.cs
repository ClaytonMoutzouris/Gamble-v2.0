using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    PlayerInventoryUI inventoryUI;
    public Player mPlayer;
    public List<Item> items;

    private void Start()
    {
        mPlayer = GetComponent<Player>();
        items = new List<Item>();
        inventoryUI = PlayerUIPanels.instance.playerPanels[mPlayer.mPlayerIndex].inventoryUI;
        inventoryUI.player = mPlayer;

    }

    public void AddItemToInventory(Item item)
    {
        items.Add(item);
        inventoryUI.AddItem(item);
    }

    public void AddItemsToInventory(List<Item> items)
    {
        this.items.AddRange(items);
        foreach(Item item in items)
        {
            inventoryUI.AddItem(item);
        }

    }

    public Item GetItemAtIndex(int index)
    {
        return items[index];
    }

    public void DropItem(int index)
    { 

        ItemObject temp = Instantiate(Resources.Load<ItemObject>("Prefabs/ItemObject")) as ItemObject;
        temp.SetItem(items[index]);
        items.Remove(items[index]);
        inventoryUI.RemoveItem(index);
        temp.EntityInit();
        temp.Body.mPosition = mPlayer.Position + new Vector3(0, MapManager.cTileSize / 2);
    }

}
