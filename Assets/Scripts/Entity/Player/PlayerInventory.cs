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
    public int size = 50;
     
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
        //check to see if the item stacks and if there already exists a stack
        if (item.isStackable)
        {
            foreach (InventorySlot slot in slots)
            {
                if (!slot.IsEmpty())
                {
                    if (slot.item.mName == item.mName)
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
        List<InventorySlotUI> indexes = new List<InventorySlotUI>();

        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i] != null) {
                if (slots[i].item is Equipment equippable && equippable.isEquipped)
                {
                    MoveItem(i, sortIndex);
                    sortIndex++;
                } 
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                if (!(slots[i].item is Equipment equippable) || !equippable.isEquipped)
                {
                    MoveItem(i, sortIndex);
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

    public void MoveItem(int prev, int dest)
    {
        if(dest > size)
        {
            //This shouldnt happen
            return;
        }

        if(slots[dest].IsEmpty())
        {

            Item temp = slots[prev].item;
            int numTemps = slots[prev].amount;
            slots[prev].ClearSlot();

            slots[dest].AddItemToSlot(temp, numTemps);

        }
        else
        {
            Item prevItem = slots[prev].item;
            int numPrevs = slots[prev].amount;

            Item destItem = slots[dest].item;
            int numDests = slots[dest].amount;

            slots[prev].ClearSlot();
            slots[dest].ClearSlot();

            slots[dest].AddItemToSlot(prevItem, numPrevs);
            slots[prev].AddItemToSlot(destItem, numDests);

        }
    }
    

    public InventorySlot FindSlotForItem(Item item)
    {
        foreach(InventorySlot slot in slots)
        {
            if(slot.item.Equals(item))
            {
                return slot;
            }
        }

        return null;
    }

}
