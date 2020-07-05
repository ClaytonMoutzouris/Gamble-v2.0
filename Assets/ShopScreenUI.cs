using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShopScreenMode { Buy, Sell }

public class ShopScreenUI : MonoBehaviour
{
    public static ShopScreenUI instance;
    public GameObject defaultObject;
    public Player currentPlayer;
    public ShopBuyNode buyPrefab;
    public ShopSellNode sellPrefab;
    public List<ShopBuyNode> shopBuyNodes;
    public List<ShopSellNode> shopSellNodes;
    public GameObject buyNodeContainer;
    public GameObject sellNodeContainer;
    public ScrollRect buyRect;
    public ScrollRect sellRect;
    public ShopTradeNode currentNode;
    public ResourceCounterUI crystalCounter;
    public NPC currentNPC;
    public ShopScreenMode buysellMode;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        gameObject.SetActive(false);
        shopBuyNodes = new List<ShopBuyNode>();


        buysellMode = ShopScreenMode.Buy;
    }

    private void Update()
    {
        if(currentNode != null)
        {
            switch (buysellMode)
            {
                case ShopScreenMode.Buy:
                    buyRect.ScrollRepositionY(currentNode.GetComponent<RectTransform>());

                    break;

                case ShopScreenMode.Sell:
                    sellRect.ScrollRepositionY(currentNode.GetComponent<RectTransform>());

                    break;
            }
        }



    }

    public void LoadInventories()
    {
        foreach (ShopBuyNode node in shopBuyNodes)
        {
            Destroy(node.gameObject);
        }

        shopBuyNodes.Clear();

        if (currentNPC != null)
        {
            foreach (Item item in currentNPC.npcWares)
            {
                ShopBuyNode temp = Instantiate(buyPrefab, buyNodeContainer.transform);
                temp.SetTradeNode(item, Resources.Load<Item>("Prototypes/Items/Elements/Blueium") as Item, item.GetValue());
                shopBuyNodes.Add(temp);
            }
        }

        foreach (ShopSellNode node in shopSellNodes)
        {
            Destroy(node.gameObject);
        }

        shopSellNodes.Clear();

        if (currentPlayer != null)
        {
            foreach(InventorySlot slot in currentPlayer.Inventory.slots)
            {
                if(!slot.IsEmpty() && slot.item.GetValue() > 0)
                {
                    ShopSellNode temp = Instantiate(sellPrefab, sellNodeContainer.transform);
                    temp.SetSellNode(slot.item, Resources.Load<Item>("Prototypes/Items/Elements/Blueium") as Item, slot.item.GetSellValue());
                    shopSellNodes.Add(temp);
                }
            }
        }

    }

    public void SetBuyMode()
    {
        buysellMode = ShopScreenMode.Buy;
        sellRect.gameObject.SetActive(false);
        buyRect.gameObject.SetActive(true);

        defaultObject = shopBuyNodes[0].gameObject;
        currentNode = shopBuyNodes[0];
        EventSystemManager.instance.GetEventSystem(currentPlayer.mPlayerIndex).SetSelectedGameObject(defaultObject);

    }

    public void SetSellMode()
    {
        buysellMode = ShopScreenMode.Sell;

        sellRect.gameObject.SetActive(true);
        buyRect.gameObject.SetActive(false);

        defaultObject = shopSellNodes[0].gameObject;
        currentNode = shopSellNodes[0];
        EventSystemManager.instance.GetEventSystem(currentPlayer.mPlayerIndex).SetSelectedGameObject(defaultObject);
    }

    public void SetCurrentNode(ShopTradeNode node)
    {
        currentNode = node;
    }

    public void Open(Player player, NPC shopkeeper)
    {
        currentNPC = shopkeeper;
        currentPlayer = player;

        LoadInventories();
        SetBuyMode();

        int counter = 0;
        foreach(InventorySlot slot in player.Inventory.slots)
        {
            if(slot.item is Element)
            {
                counter += slot.amount;
            }
        }
        crystalCounter.SetCounter(counter);
        defaultObject.GetComponent<Button>().OnSelect(null);
        gameObject.SetActive(true);
        currentPlayer.Input.inputState = PlayerInputState.Shop;
        defaultObject.GetComponent<Button>().OnSelect(null);


    }

    public void Close()
    {
        EventSystemManager.instance.GetEventSystem(currentPlayer.mPlayerIndex).SetSelectedGameObject(null);
        gameObject.SetActive(false);
        currentPlayer.Input.inputState = PlayerInputState.Game;
        currentPlayer = null;
        crystalCounter.SetCounter(0);
        currentNPC = null;
    }

    public void SelectNode(ShopTradeNode node)
    {
        switch(buysellMode)
        {
            case ShopScreenMode.Buy:
                ShopBuyNode buyNode = (ShopBuyNode)node;
                if(currentPlayer.GetCurrency(buyNode.GetCurrency(), buyNode.GetCost())) {
                    currentPlayer.Inventory.AddItemToInventory(ItemDatabase.NewItem(buyNode.GetOffer()));
                    crystalCounter.SetCounter(currentPlayer.GetCurrencyCount(buyNode.GetCurrency()));
                }

                break;

            case ShopScreenMode.Sell:
                ShopSellNode sellNode = (ShopSellNode)node;
                if (currentPlayer.GetCurrency(sellNode.GetItemForSale(), 1))
                {
                    for(int i = sellNode.GetValue(); i > 0; i--)
                    {
                        currentPlayer.Inventory.AddItemToInventory(ItemDatabase.NewItem(sellNode.GetCurrency()));
                    }
                    crystalCounter.SetCounter(currentPlayer.GetCurrencyCount(sellNode.GetCurrency()));
                }

                break;
        }


    }
}
