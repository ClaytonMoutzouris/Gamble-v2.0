using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScreenUI : MonoBehaviour
{
    public static ShopScreenUI instance;
    public GameObject defaultObject;
    public int pausedIndex = -1;
    public ShopTradeNode prefab;
    public List<Item> items;
    public List<ShopTradeNode> shopTradeNodes;
    public GameObject nodeContainer;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        gameObject.SetActive(false);

        SetShopTradeNodes();

        defaultObject = shopTradeNodes[0].gameObject;


    }

    public void SetShopTradeNodes()
    {
        shopTradeNodes = new List<ShopTradeNode>();

        foreach(Item item in items)
        {
            ShopTradeNode temp = Instantiate(prefab, nodeContainer.transform);
            temp.SetTradeNode(item, Resources.Load<Item>("Prototypes/Items/Elements/Blueium") as Item, 5);
            shopTradeNodes.Add(temp);
        }

    }

    public void Open(int playerIndex)
    {

        pausedIndex = playerIndex;
        EventSystemManager.instance.GetEventSystem(pausedIndex).SetSelectedGameObject(defaultObject);
        defaultObject.GetComponent<Button>().OnSelect(null);
        gameObject.SetActive(true);
        CrewManager.instance.players[playerIndex].Input.inputState = PlayerInputState.Shop;
        defaultObject.GetComponent<Button>().OnSelect(null);


    }

    public void Close()
    {
        EventSystemManager.instance.GetEventSystem(pausedIndex).SetSelectedGameObject(null);
        gameObject.SetActive(false);
        CrewManager.instance.players[pausedIndex].Input.inputState = PlayerInputState.Game;
        pausedIndex = -1;

    }

    public void SelectNode(ShopTradeNode node)
    {
        Debug.Log(node.offerNode.text.text + " was selected");
        InventorySlot slot = CrewManager.instance.players[pausedIndex].Inventory.GetSlotWithItemType(node.costNode.item);
        if (slot != null)
        {
            if(slot.amount >= node.costNode.cost)
            {
                for(int i = 0; i < node.costNode.cost; i++)
                {
                    slot.GetOneItem();
                }
                Debug.Log("Adding " + node.offerNode.item.mName + " To inventory");
                Debug.Log("Adding " + node.offerNode.text.text + " To inventory");

                CrewManager.instance.players[pausedIndex].Inventory.AddItemToInventory(ItemDatabase.NewItem(node.offerNode.item));

            }
        }
    }
}
