﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventorySlot : MonoBehaviour
{
    public PlayerInventoryUI playerInventory;
    public int slotID;
    public Button button;
    public Item item = null;
    public bool isEquipped = false;
    [SerializeField]
    GameObject equipFlag;
    [SerializeField]
    Image mImage;

    public void SetSlot(int id, PlayerInventoryUI inv)
    {
        slotID = id;
        playerInventory = inv;
    }

    public void Awake()
    {
        mImage.color  = Color.clear;
        button = GetComponent<Button>();
        equipFlag.SetActive(false);
    }

    public void SetItem(Item i)
    {
        if(i == null)
        {
            Debug.LogError("Item is trying to be set as null");
            return;
        }

        item = i;
        item.SetInventorySlot(this);
        mImage.sprite = item.sprite;
        mImage.color = Color.white;   

    }

    public void ClearItem()
    {
        if(item == null)
        {
            return;
        }


        item.SetInventorySlot(null);
        item = null;
        SetEquipped(false);


        mImage.sprite = null;
        mImage.color = Color.clear;

    }

    public bool IsEmpty()
    {
        return item == null;
    }

    public Item GetItem()
    {
        return item;
    }

    public void SetEquipped(bool equipped)
    {
        isEquipped = equipped;
        if(isEquipped)
        {
            equipFlag.SetActive(true);
        } else
        {
            equipFlag.SetActive(false);
        }
    }

    public void SlotSelected()
    {
        playerInventory.SlotSelected(this);

    }

}
