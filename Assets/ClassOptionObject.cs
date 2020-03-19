using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClassOptionObject : ScrollListButton
{
    public PlayerClassType classType;

    // Start is called before the first frame update
    void Start()
    {
        scrollParent = scrollList.GetComponent<ScrollRect>();
        Debug.Log("Scroll parent null? " + scrollParent == null);
        text.text = classType.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        scrollList.parentPanel.classType = classType;
    }
}
