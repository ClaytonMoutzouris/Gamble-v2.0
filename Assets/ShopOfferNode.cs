﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopOfferNode : MonoBehaviour
{
    public Item item;
    public Text text;
    public Image image;

    public void Start()
    {
        if (item != null)
        {
            SetShopOfferNode(item);
        }
    }

    public void SetShopOfferNode(Item item)
    {
        Debug.Log("Setting shop offer node with " + item.mName);
        this.item = item;
        text.text = item.mName;
        image.sprite = item.sprite;
    }


}
