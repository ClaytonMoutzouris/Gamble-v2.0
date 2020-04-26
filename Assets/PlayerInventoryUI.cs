using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class PlayerInventoryUI : MonoBehaviour
{
    public Player player;
    public PlayerPanel panel;
    public int numStartingSlots = 50;
    public List<InventorySlotUI> slots;
    public InventorySlotUI slotPrefab;
    public int columns;
    public InventoryOptionList optionsList;

    public Selectable interactableUp;
    public bool selectionMode = false;
    public InventorySlotUI moving;
    public InventorySlotUI currentSlot;

    public void Initialize()
    {
        Debug.Log("Starting Player Inventory UI");
        Debug.Log("Starting Slots " + numStartingSlots);

        slots = new List<InventorySlotUI>();

        for(int i = 0; i < numStartingSlots; i++)
        {
            AddSlot();
        }

    }

    InventorySlotUI getSlot(int x, int y)
    {
        return slots[y * columns + x];
    }

    InventorySlotUI AddSlot()
    {
        InventorySlotUI slot = Instantiate(slotPrefab, transform);
        Navigation customNav = slot.GetComponent<Button>().navigation;
        Navigation leftNav, upNav;
        int index = slots.Count;
        int y = index / columns;
        int x = index % columns;

        if (y == 0)
        {
            customNav.selectOnUp = interactableUp;
            upNav = interactableUp.navigation;
            if (x == 0)
            {
                upNav.selectOnDown = slot.GetComponent<Button>();
                interactableUp.navigation = upNav;
            }
        } else
        {
            customNav.selectOnUp = getSlot(x, y - 1).GetComponent<Button>();
            upNav = getSlot(x, y-1).GetComponent<Button>().navigation;
            upNav.selectOnDown = slot.GetComponent<Button>();
            getSlot(x, y - 1).GetComponent<Button>().navigation = upNav;

        }

        if (x == 0)
        {
            
        } else
        {
            customNav.selectOnLeft = getSlot(x-1, y).GetComponent<Button>();
            leftNav = getSlot(x-1, y).GetComponent<Button>().navigation;
            leftNav.selectOnRight = slot.GetComponent<Button>();
            getSlot(x - 1, y).GetComponent<Button>().navigation = leftNav;
        }

        slot.GetComponent<Button>().navigation = customNav;
        slots.Add(slot);
        slot.SetSlot(index, panel);
        return slot;
    }

    public void MoveItem(int index)
    {
        if(slots[index].slot.item == null)
        {
            return;
        }
        moving = slots[index];
        selectionMode = true;
    }

    public void OpenOptionsList(InventorySlotUI slot)
    {
        if (slot.slot.IsEmpty())
        {
            return;
        }
        optionsList.OpenOptions(slot);
    }

    public void SlotSelected(InventorySlotUI slot)
    {
        if(selectionMode)
        {
            if(moving.slotID != slot.slotID)
            {
                player.Inventory.MoveItem(moving.slotID, slot.slotID);
            }

            moving = null;
            selectionMode = false;
        }
        else
        {
            OpenOptionsList(slot);
        }
    }
    
}

