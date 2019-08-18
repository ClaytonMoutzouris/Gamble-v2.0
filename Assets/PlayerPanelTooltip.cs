using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelTooltip : MonoBehaviour
{
    public Text tooltipText;

    public void SetTooptip(string tooltip)
    {
        tooltipText.text = tooltip;

    }

    public void ClearTooltip()
    {
        tooltipText.text = "";
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}
