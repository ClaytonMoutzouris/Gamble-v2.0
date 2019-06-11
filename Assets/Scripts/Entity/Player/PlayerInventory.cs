using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerInventory
{
    public PlayerInventoryUI inventoryUI;
    public Player mPlayer;
    public List<Item> items;
     
    public PlayerInventory(Player player)
    {
        mPlayer = player;
        items = new List<Item>();
        inventoryUI = PlayerUIPanels.instance.playerPanels[mPlayer.mPlayerIndex].inventoryUI;
        inventoryUI.player = mPlayer;

    }


    public void AddItemToInventory(Item item)
    {

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                inventoryUI.AddItem(item, i);
                return;
            }
        }

        items.Add(item);
        inventoryUI.AddItem(item, items.Count - 1);

    }
    /*
    public void AddItemsToInventory(List<Item> items)
    {
        this.items.AddRange(items);
        foreach(Item item in items)
        {
            inventoryUI.AddItem(item);
        }

    }
    */
    public Item GetItemAtIndex(int index)
    {
        return items[index];
    }

    public bool UseItem(int index)
    {
        Debug.Log("User Item:" + index);
        if (items[index] is ConsumableItem)
        {
            ((ConsumableItem)items[index]).Use(mPlayer, index);
            

            return true;
        }

        return false;
    }

    public void RemoveItem(int index)
    {
        items[index] = null;
        inventoryUI.RemoveItem(index);
    }

    public void DropItem(int index)
    {
        ItemObject temp = (ItemObject)LevelManager.instance.AddEntity(Resources.Load<ItemObject>("Prefabs/ItemObject"));
        temp.SetItem(items[index]);
        items[index] = null;
        inventoryUI.RemoveItem(index);
        temp.EntityInit();
        temp.Body.mPosition = mPlayer.Position + new Vector3(0, MapManager.cTileSize / 2);
    }

    public void EquipItem(int index)
    {
        if(items[index] is Equipment)
        {
            Equipment equippable = (Equipment)items[index];
            if (mPlayer.mEquipment.Equip(equippable))
            {
                inventoryUI.EquipItem(index);
            }
            

        }
    }

}
