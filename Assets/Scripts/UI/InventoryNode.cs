using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryNode : MonoBehaviour
{
    Item item;
    int indexInInventory;
    public Image mImage;

    private void Awake()
    {
        if(mImage == null)
        mImage = GetComponent<Image>();


        indexInInventory = -1;
    }

    public void SetItem(Item i)
    {
        item = i;



        mImage.sprite = item.sprite;
    }

    public Item GetItem()
    {
        return item;
    }
}