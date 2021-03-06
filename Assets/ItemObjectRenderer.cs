﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemObjectRenderer : EntityRenderer
{
    public TextMesh nameDisplay;
    public Item item;

    protected override void Awake()
    {
        base.Awake();
        nameDisplay.gameObject.SetActive(false);
        nameDisplay.gameObject.GetComponent<MeshRenderer>().sortingLayerName = "UI";
    }

    public void SetItem(Item item)
    {
        this.item = item;
    }

    public void ShowText()
    {
        
        nameDisplay.text = "<color=" + item.GetColorStringFromRarity() + ">" + item.itemName + "</color>";
        nameDisplay.gameObject.SetActive(true);

    }

    public void HideText()
    {
        nameDisplay.gameObject.SetActive(false);
    }

}
