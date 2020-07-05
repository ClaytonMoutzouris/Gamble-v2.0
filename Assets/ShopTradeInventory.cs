using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTradeInventory : MonoBehaviour
{
    public GameObject container;
    public ShopBuyNode prefab;
    public ScrollRect scrollRect;
    public List<ShopBuyNode> tradeNodes;

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }


    public void AddShopTradeNode(Item item, Item trade, int cost)
    {
        ShopBuyNode node = Instantiate(prefab, container.transform);
        node.SetTradeNode(item, trade, cost);
        tradeNodes.Add(node);
    }

    public void NodeSelected(ShopBuyNode node)
    {

    }


}
