using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventorySlotUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public PlayerPanel playerPanel;
    public int slotID;
    public Button button;
    [SerializeField]
    GameObject equipFlag;
    [SerializeField]
    Image mImage;
    public Text stackSizeText;
    public InventorySlot slot;

    public void SetSlot(int id, PlayerPanel panel)
    {
        slotID = id;
        playerPanel = panel;
    }

    public void Awake()
    {
        mImage.color  = Color.clear;
        button = GetComponent<Button>();
        equipFlag.SetActive(false);
        stackSizeText.text = "";
    }

    public void UpdateSlotUI()
    {
        if (slot.item != null)
        {
            mImage.sprite = slot.item.sprite;
            mImage.color = Color.white;
            if(slot.amount > 1)
            {
                stackSizeText.text = "" + slot.amount;
            } else
            {
                stackSizeText.text = "";
            }

            if (slot.item is Equipment equipment)
                equipFlag.SetActive(equipment.isEquipped);
        }
        else
        {
            mImage.sprite = null;
            mImage.color = Color.clear;
            equipFlag.SetActive(false);
            stackSizeText.text = "";
        }


    }

    public void SlotSelected()
    {
        playerPanel.inventoryUI.SlotSelected(this);

    }

    public void OnSelect(BaseEventData eventData)
    {
        //Show tooltip in playerpanel
        if (slot.item != null)
        {
            playerPanel.tooltip.SetTooptip(slot.item.GetTooltip());
        }

        playerPanel.inventoryUI.currentSlot = this;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        playerPanel.tooltip.ClearTooltip();
    }
}
