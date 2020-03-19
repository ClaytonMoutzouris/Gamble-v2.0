using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class ScrollListButton : MonoBehaviour, ISelectHandler
{
    public ScrollList scrollList;
    protected ScrollRect scrollParent;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        scrollParent = scrollList.GetComponent<ScrollRect>();
        Debug.Log("Scroll parent null? " + scrollParent == null);
        //scrollParent.content.localPosition = scrollParent.GetSnapToPositionToBringChildIntoView(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        scrollParent.content.localPosition = scrollParent.GetSnapToPositionToBringChildIntoView(transform);
    }
}
