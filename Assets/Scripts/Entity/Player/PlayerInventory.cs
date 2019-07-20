using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerInventory
{
    public PlayerInventoryUI inventoryUI;
    public Player mPlayer;
    public Item[] items;
    public int size = 50;
     
    public PlayerInventory(Player player)
    {
        mPlayer = player;

        inventoryUI = PlayerUIPanels.instance.playerPanels[mPlayer.mPlayerIndex].inventoryUI;
        inventoryUI.player = mPlayer;
        items = new Item[size];
    }


    public bool AddItemToInventory(Item item)
    {

        for (int i = 0; i < size; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                inventoryUI.AddItem(item, i);
                return true;
            }
        }

        /*
        items.Add(item);
        inventoryUI.AddItem(item, items.Count - 1);
        */

        return false;

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
    
    public void AddItem(Item item, int index)
    {
        items[index] = item;
        inventoryUI.AddItem(item, index);
    }

    public void RemoveItem(int index)
    {
        items[index] = null;
        inventoryUI.RemoveItem(index);
    }

    public void DropItem(int index)
    {
        ItemObject temp = new ItemObject(items[index]);
        temp.Spawn(mPlayer.Position + new Vector2(0, MapManager.cTileSize / 2));
        items[index] = null;
        inventoryUI.RemoveItem(index);
        //temp.Body.mPosition = mPlayer.Position + new Vector3(0, MapManager.cTileSize / 2);
    }

    public void EquipItem(int index)
    {
        if (items[index].GetInventorySlot().isEquipped)
        {
            return;
        }

        if(items[index] is Equipment)
        {
            Equipment equippable = (Equipment)items[index];
            if (mPlayer.Equipment.EquipItem(equippable))
            {
                inventoryUI.EquipItem(index);
            }
            

        }
    }

    public void UnequipItem(int index)
    {
        if (items[index] is Equipment)
        {
            Equipment equippable = (Equipment)items[index];
            mPlayer.Equipment.Unequip(equippable.mSlot);
        }
    }

    public void MoveItem(int prev, int dest)
    {
        if(dest > size)
        {
            //This shouldnt happen
            return;
        }

        if(items[dest] != null)
        {
            Item prevItem = items[prev];
            Item destItem = items[dest];
            bool prevEquipped = items[prev].GetInventorySlot().isEquipped;
            bool destEquipped = items[dest].GetInventorySlot().isEquipped;

            RemoveItem(prev);
            RemoveItem(dest);

            AddItem(destItem, prev);
            items[prev].GetInventorySlot().SetEquipped(destEquipped);

            AddItem(prevItem, dest);
            items[dest].GetInventorySlot().SetEquipped(prevEquipped);


        }
        else
        {
            Item temp = items[prev];
            InventorySlot slot = items[prev].GetInventorySlot();
            bool equipped = inventoryUI.slots[prev].isEquipped;

            RemoveItem(prev);

            AddItem(temp, dest);
            inventoryUI.slots[dest].SetEquipped(equipped);

        }
    }
    


}
