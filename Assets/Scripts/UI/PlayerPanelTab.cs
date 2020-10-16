using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerPanelTab : MonoBehaviour
{

    public Player player;
    public PlayerPanel panel;
    public GameObject defaultSelection;
    public GameObject tabButton;

    public virtual void Open()
    {
        gameObject.SetActive(true);
        if(player != null)
        {
            GamepadInputManager.instance.gamepadInputs[player.mPlayerIndex].GetEventSystem().SetSelectedGameObject(defaultSelection);
        }

    }

    public virtual void Close()
    {
        gameObject.SetActive(false);

    }

    public virtual void UpdateTab()
    {

    }
}
