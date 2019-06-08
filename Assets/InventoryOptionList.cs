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

        Navigation n;
        for (int i = 0; i < Enum.GetValues(typeof(InventoryOption)).Length; i++)
        {
            InventoryOptionButton optionButton = Instantiate(optionPrefab, transform);
            optionButton.SetOption((InventoryOption)i, this);
            options.Add(optionButton);

            if(i > 0)
            {
                n = optionButton.button.navigation;
                n.selectOnUp = options[i-1].button;
                optionButton.button.navigation = n;

                n = options[i-1].button.navigation;
                n.selectOnDown = optionButton.button;
                options[i-1].button.navigation = n;
            }
        }

        n = options[0].button.navigation;
        n.selectOnUp = options[options.Count-1].button;
        options[0].button.navigation = n;

        n = options[options.Count - 1].button.navigation;
        n.selectOnDown = options[0].button;
        options[options.Count - 1].button.navigation = n;

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOptions(InventorySlot slot)
    {
        focusedSlot = slot;
        gameObject.SetActive(true);
        transform.position = slot.transform.position;
        EventSystemManager.instance.GetEventSystem(playerInventory.player.playerIndex).SetSelectedGameObject(options[0].gameObject);
    }

    public void OptionSelected(InventoryOption option)
    {
        switch (option)
        {
            case InventoryOption.Drop:
                playerInventory.player.mInventory.DropItem(focusedSlot.slotID);
                break;
            case InventoryOption.Cancel:
                
                break;
        }

        Close();
    }

    public void Close()
    {
        EventSystemManager.instance.GetEventSystem(playerInventory.player.playerIndex).SetSelectedGameObject(focusedSlot.button.gameObject);
        gameObject.SetActive(false);
    }
}
