using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopCostNode : MonoBehaviour
{
    public Item item;
    public int cost;
    public Text text;
    public Image image;

    public void Start()
    {
        if(item != null)
        {
            SetShopCostNode(item, cost);
        }
    }

    public void SetShopCostNode(Item item, int cost)
    {
        this.cost = cost;
        this.item = item;
        text.text = cost + "X " + item.mName;
        image.sprite = item.sprite;
    }


}
