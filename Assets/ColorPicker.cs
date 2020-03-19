using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class ColorPicker : MonoBehaviour, ISelectHandler
{
    List<ColorPickerNode> nodes;

    // Start is called before the first frame update
    void Start()
    {
        nodes = new List<ColorPickerNode>();
        ColorPickerNode[] children = gameObject.GetComponentsInChildren<ColorPickerNode>();
        for(int i = 0; i < children.Length; i++)
        {
            nodes.Add(children[i]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenColorPicker()
    {

    }

    public void OnSelect(BaseEventData eventData)
    {
        //nodes[0].OnSelect(eventData);
    }
}
