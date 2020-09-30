using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInventory
{
    public PlayerInventoryUI inventoryUI;
    public Player mPlayer;
    public InventorySlot[] slots;
    public int size = 42;
     
    public PlayerInventory(Player player)
    {
        mPlayer = player;

        inventoryUI = PlayerUIPanels.instance.playerPanels[mPlayer.mPlayerIndex].inventoryUI;
        inventoryUI.player = mPlayer;
        slots = new InventorySlot[size];

        for (int i = 0; i < size; i++)
        {
            slots[i] = new InventorySlot(this);
            slots[i].SetInventorySlot(inventoryUI.slots[i]);
        }
    }


    public bool AddItemToInventory(Item item)
    {
        if(item == null)
        {
            return false;
        }
        //check to see if the item stacks and if there already exists a stack
        if (item.isStackable)
        {
            foreach (InventorySlot slot in slots)
            {
                if (!slot.IsEmpty())
                {
                    if (slot.item.itemName == item.itemName)
                    {
                        slot.AddItemToSlot(item);
                        return true;
                    }
                }
            }
        }
        
        //Get the first open space
        foreach (InventorySlot slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.AddItemToSlot(item);
                return true;
            }
        }

        return false;

    }

    public void SortInventory()
    {
        int sortIndex = 0;

        for(int i = 0; i < slots.Length; i++)
        {
            if(!slots[i].IsEmpty()) {
                if (slots[i].item is Equipment equippable && equippable.isEquipped)
                {
                    MoveItem(slots[i], slots[sortIndex]);
                    sortIndex++;
                } 
            }
        }

        
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].IsEmpty())
            {
                if (!(slots[i].item is Equipment equippable) || !equippable.isEquipped)
                {
                    MoveItem(slots[i], slots[sortIndex]);
                    sortIndex++;
                }
            }
        }
        
    }

    public InventorySlot FindKeySlot()
    {

        foreach(InventorySlot slot in slots)
        {
            if(slot.item is Key)
            {
                return slot;
            }
        }

        return null;
    }

    public void MoveItem(InventorySlot toMove, InventorySlot destination)
    {
        if(toMove == destination)
        {
            return;
        }

        if(destination.IsEmpty())
        {

            Item temp = toMove.item;
            int numTemps = toMove.amount;
            toMove.ClearSlot();

            destination.AddItemToSlot(temp, numTemps);

        }
        else if(destination.item.isStackable && destination.item.itemName.Equals(toMove))
        {
            Item prevItem = toMove.item;
            int numPrevs = toMove.amount;
            toMove.ClearSlot();

            destination.AddItemToSlot(prevItem, numPrevs);
        } else
        {
            Item prevItem = toMove.item;
            int numPrevs = toMove.amount;

            Item destItem = destination.item;
            int numDests = destination.amount;

            toMove.ClearSlot();
            destination.ClearSlot();

            destination.AddItemToSlot(prevItem, numPrevs);
            toMove.AddItemToSlot(destItem, numDests);

        }
    }
    

    public InventorySlot FindSlotForItem(Item item)
    {
        foreach(InventorySlot slot in slots)
        {
            if(!slot.IsEmpty() && slot.item.Equals(item))
            {
                return slot;
            }
        }

        return null;
    }

    public InventorySlot GetSlotWithItemType(Item item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty())
            {
                if (slot.item.itemName == item.itemName)
                {
                    return slot;
                }
            }
        }

        return null;
    }

}
