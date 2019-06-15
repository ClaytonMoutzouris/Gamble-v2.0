using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatContainer : MonoBehaviour
{
    public List<UIStatElement> uIStatElements;
    public Button interactableUp;


    public void Start()
    {
        uIStatElements = new List<UIStatElement>();
        foreach(UIStatElement element in GetComponentsInChildren<UIStatElement>())
        {
            uIStatElements.Add(element);
        }

        Navigation n;
        for (int i = 0; i < uIStatElements.Count; i++)
        {
            Navigation upNav;
            n = uIStatElements[i].button.navigation;

            if (i == 0)
            {
                n.selectOnUp = interactableUp;
                uIStatElements[i].button.navigation = n;

                upNav = interactableUp.navigation;
                upNav.selectOnDown = uIStatElements[i].button;
                interactableUp.navigation = upNav;
            }
            else
            {
                n.selectOnUp = uIStatElements[i - 1].button;
                uIStatElements[i].button.navigation = n;

                n = uIStatElements[i - 1].button.navigation;
                n.selectOnDown = uIStatElements[i].button;
                uIStatElements[i - 1].button.navigation = n;
            }
        }

    }


    public void SetStat(Stat stat)
    {
        uIStatElements[(int)stat.type].SetStat(stat);
    }
}
