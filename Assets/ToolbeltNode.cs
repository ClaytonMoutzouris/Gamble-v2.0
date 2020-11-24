using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolbeltNode : MonoBehaviour
{
    public Image nodeIcon;

    public void SetNode(Item item)
    {
        nodeIcon.sprite = item.sprite;
    }

}
