using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryOption { Use, Equip, Unequip, Move, Drop, Cancel };

public class InventoryOptionList : MonoBehaviour
{
    public PlayerInventoryUI playerInventory;
    public List<InventoryOptionButton> options;
    public InventoryOptionButton optionPrefab;
    public InventorySlotUI focusedSlot;

    public void ClearOptions()
    {
        for(int i = options.Count-1; i >= 0; i--)
        {
            Destroy(options[i].gameObject);
        }

        options.Clear();
    }

    public void SetOptions()
    {
        ClearOptions();
        if (focusedSlot == null)
        {
            return;
        }

        List<InventoryOption> validOptions = focusedSlot.slot.item.GetInventoryOptions();

        if(validOptions.Contains(InventoryOption.Equip) && focusedSlot.slot.item is Equipment equippable && equippable.isEquipped)
        {
            validOptions.Remove(InventoryOption.Equip);
            validOptions.Insert(0, InventoryOption.Unequip);
        }

        if(validOptions.Count == 0)
        {
            return;
        }

        Navigation n;
        for (int i = 0; i < validOptions.Count; i++)
        {
            InventoryOptionButton optionButton = Instantiate(optionPrefab, transform);
            optionButton.SetOption(validOptions[i], this);
            options.Add(optionButton);

            if (i > 0)
            {
                n = optionButton.button.navigation;
                n.selectOnUp = options[i - 1].button;
                optionButton.button.navigation = n;

                n = options[i - 1].button.navigation;
                n.selectOnDown = optionButton.button;
                options[i - 1].button.navigation = n;
            }
        }

        n = options[0].button.navigation;
        n.selectOnUp = options[options.Count - 1].button;
        options[0].button.navigation = n;

        n = options[options.Count - 1].button.navigation;
        n.selectOnDown = options[0].button;
        options[options.Count - 1].button.navigation = n;

    }

    public void OpenOptions(InventorySlotUI slot)
    {
        focusedSlot = slot;
        SetOptions();

        if (options.Count == 0)
        {
            return;
        }
        gameObject.SetActive(true);
        transform.position = slot.transform.position;
        EventSystemManager.instance.GetEventSystem(playerInventory.player.mPlayerIndex).SetSelectedGameObject(options[0].gameObject);
        options[0].gameObject.GetComponent<Button>().OnSelect(null);

    }

    public void OptionSelected(InventoryOption option)
    {
        switch (option)
        {
            case InventoryOption.Drop:
                focusedSlot.slot.DropItem();
                break;
            case InventoryOption.Use:
                focusedSlot.slot.UseItem();

                break;
            case InventoryOption.Equip:
                focusedSlot.slot.EquipItem();

                break;
            case InventoryOption.Unequip:
                focusedSlot.slot.UnequipItem();

                break;
            case InventoryOption.Move:
                playerInventory.MoveItem(focusedSlot.slotID);
                break;
        }

        Confirm();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Confirm()
    {
        EventSystemManager.instance.GetEventSystem(playerInventory.player.mPlayerIndex).SetSelectedGameObject(focusedSlot.button.gameObject);
        gameObject.SetActive(false);
    }

}
