using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonTooltip : MonoBehaviour
{
    public TextMeshPro text;
    public SpriteRenderer buttonIcon;

    private void Start()
    {

    }

    public void SetText(string text)
    {
        this.text.SetText(text);
    }

    public void ShowTooltip(bool show)
    {
        gameObject.SetActive(show);
    }
}
