using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class ColorPickerNode : ScrollListButton
{
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        scrollParent = scrollList.GetComponent<ScrollRect>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        scrollList.parentPanel.SwapColor(image.color);
    }

}
