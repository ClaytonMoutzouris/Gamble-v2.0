using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonTooltip : MonoBehaviour
{
    public TextMesh text;

    private void Start()
    {
        text.gameObject.GetComponent<MeshRenderer>().sortingLayerName = "UI";

    }

    public void SetText(string text)
    {
        this.text.text = text;

    }

    public void ShowTooltip(bool show)
    {
        gameObject.SetActive(show);
    }
}
