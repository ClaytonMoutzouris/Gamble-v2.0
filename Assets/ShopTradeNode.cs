using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopTradeNode : MonoBehaviour
{

    public ShopCostNode costNode;
    public ShopOfferNode offerNode;
    public Button button;

    public void SelectOption()
    {
        ShopScreenUI.instance.SelectNode(this);
    }

    public void SetTradeNode(Item offer, Item costItem, int cost)
    {
        offerNode.SetShopOfferNode(offer);
        costNode.SetShopCostNode(costItem, cost);

    }


}
