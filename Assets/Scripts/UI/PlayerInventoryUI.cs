using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class PlayerInventoryUI : MonoBehaviour
{
    public Player player;
    public int numStartingSlots;
    public List<InventorySlot> slots;
    public InventorySlot slotPrefab;
    public int columns;
    public InventoryOptionList optionsList;

    public Selectable interactableUp;

    private void Start()
    {
        slots = new List<InventorySlot>();

        for(int i = 0; i < numStartingSlots; i++)
        {
            AddSlot();
        }
    }

    InventorySlot getSlot(int x, int y)
    {
        return slots[y * columns + x];
    }

    InventorySlot AddSlot()
    {
        InventorySlot slot = Instantiate(slotPrefab, transform);
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
        slot.SetSlot(index, this);
        return slot;
    }

    public InventorySlot getFirstEmpty()
    {
        foreach (InventorySlot slot in slots)
        {
            if(slot.GetItem() == null)
            {
                return slot;
            }
        }

        return AddSlot();
    }

    public void AddItem(Item item)
    {
        getFirstEmpty().SetItem(item);

    }

    public void RemoveItem(int index)
    {
        slots[index].ClearItem();
    }

    public void OpenOptionsList(InventorySlot slot)
    {
        optionsList.SetOptions(slot);
    }
    
}

