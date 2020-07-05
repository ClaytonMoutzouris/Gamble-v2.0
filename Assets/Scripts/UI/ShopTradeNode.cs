using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopTradeNode : MonoBehaviour, ISelectHandler
{
    public Button button;

    public void OnSelect(BaseEventData eventData)
    {
        ShopScreenUI.instance.SetCurrentNode(this);
    }

    public void SelectOption()
    {
        ShopScreenUI.instance.SelectNode(this);
    }
}
