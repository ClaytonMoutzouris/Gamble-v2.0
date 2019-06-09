using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryOption { Use, Equip, Move, Drop, Cancel };

public class InventoryOptionList : MonoBehaviour
{
    public PlayerInventoryUI playerInventory;
    public List<InventoryOptionButton> options;
    public InventoryOptionButton optionPrefab;
    public InventorySlot focusedSlot;

    // Start is called before the first frame update
    void Start()
    {
        options = new List<InventoryOptionButton>();

        SetOptions();

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

        List<InventoryOption> validOptions = focusedSlot.item.GetInventoryOptions();

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

    public void OpenOptions(InventorySlot slot)
    {
        focusedSlot = slot;
        SetOptions();
        if(options.Count == 0)
        {
            return;
        }
        gameObject.SetActive(true);
        transform.position = slot.transform.position;
        EventSystemManager.instance.GetEventSystem(playerInventory.player.mPlayerIndex).SetSelectedGameObject(options[0].gameObject);
    }

    public void OptionSelected(InventoryOption option)
    {
        switch (option)
        {
            case InventoryOption.Drop:
                playerInventory.player.mInventory.DropItem(focusedSlot.slotID);
                break;
            case InventoryOption.Use:
                playerInventory.player.mInventory.UseItem(focusedSlot.slotID);

                break;
        }

        Close();
    }

    public void Close()
    {
        EventSystemManager.instance.GetEventSystem(playerInventory.player.mPlayerIndex).SetSelectedGameObject(focusedSlot.button.gameObject);
        gameObject.SetActive(false);
    }
}
