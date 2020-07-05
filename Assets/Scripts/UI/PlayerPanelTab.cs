using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerPanelTab : MonoBehaviour
{

    public Player player;
    public PlayerPanel panel;
    public GameObject defaultSelection;

    public virtual void Open()
    {
        gameObject.SetActive(true);
        if(player != null)
        {
            EventSystemManager.instance.GetEventSystem(player.mPlayerIndex).SetSelectedGameObject(defaultSelection);
        }

    }

    public virtual void Close()
    {
        gameObject.SetActive(false);

    }
}
