using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalCoop;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour {

    public static PauseMenu instance;
    public GameObject defaultObject;
    public int pausedIndex = -1;

    public void Start()
    {

        instance = this;
        gameObject.SetActive(false);
    }

    public void Open(int playerIndex)
    {
        pausedIndex = playerIndex;
        EventSystemManager.instance.GetEventSystem(pausedIndex).SetSelectedGameObject(defaultObject);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        EventSystemManager.instance.GetEventSystem(pausedIndex).SetSelectedGameObject(null);
        pausedIndex = -1;
        gameObject.SetActive(false);
    }

    public void DropPlayer()
    {
        if (pausedIndex == -1)
            return;

        LevelManager.instance.DropPlayer(pausedIndex);
    }

}
