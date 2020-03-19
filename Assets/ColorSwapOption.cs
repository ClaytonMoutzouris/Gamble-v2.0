using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class ColorSwapOption : ScrollListButton
{
    public SwapIndex swapIndex;

    // Start is called before the first frame update
    void Start()
    {
        scrollParent = scrollList.GetComponent<ScrollRect>();
        Debug.Log("Scroll parent null? " + scrollParent == null);
        text = GetComponentInChildren<Text>();
        text.text = swapIndex.ToString();
    }

    // Update is c



   void Update()
    {

    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        scrollList.parentPanel.swapIndex = swapIndex;
    }
}
