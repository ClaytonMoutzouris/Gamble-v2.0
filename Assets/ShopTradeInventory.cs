using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTradeInventory : MonoBehaviour
{


    public void AddShopTradeNode(Item item, Item trade, int cost)
    {
        ShopTradeNode node = new ShopTradeNode();
        node.SetTradeNode(item, trade, cost);
    }

    public void NodeSelected(ShopTradeNode node)
    {

    }

}
