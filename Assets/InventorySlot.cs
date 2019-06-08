using System.Collections;
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
    }

    public void SetItem(Item i)
    {
        item = i;
        mImage.sprite = item.sprite;
        mImage.color = Color.white;

    }

    public void ClearItem()
    {
        item = null;
        mImage.sprite = null;
        mImage.color = Color.clear;

    }

    public bool isEmpty()
    {
        return item == null;
    }

    public Item GetItem()
    {
        return item;
    }

    public void SlotSelected()
    {
        if (!isEmpty())
        {
            playerInventory.OpenOptionsList(this);
        }
    }

}
