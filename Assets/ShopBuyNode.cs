using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopBuyNode : ShopTradeNode, ISelectHandler
{

    public ShopCostNode costNode;
    public ShopOfferNode offerNode;

    public void SetTradeNode(Item offer, Item costItem, int cost)
    {
        offerNode.SetShopOfferNode(offer);
        costNode.SetShopCostNode(costItem, cost);

    }

    public int GetCost()
    {
        return costNode.cost;
    }

    public Item GetCurrency()
    {
        return costNode.item;
    }

    public Item GetOffer()
    {
        return offerNode.item;
    }

}
