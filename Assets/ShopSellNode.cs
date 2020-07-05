using System.Collections;
using System.Collections.Generic;


public class ShopSellNode : ShopTradeNode
{

    public ShopCostNode costNode;
    public ShopOfferNode offerNode;


    public void SetSellNode(Item offer, Item currency, int value)
    {
        offerNode.SetShopOfferNode(offer);
        costNode.SetShopCostNode(currency, value);

    }

    public int GetValue()
    {
        return costNode.cost;
    }

    public Item GetCurrency()
    {
        return costNode.item;
    }

    public Item GetItemForSale()
    {
        return offerNode.item;
    }

}
