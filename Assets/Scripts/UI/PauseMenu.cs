using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LocalCoop;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour {

    public static PauseMenu instance;
    public GameObject defaultObject;
    public int pausedIndex = -1;
    public List<GameObject> menuPanels;
    public int activePanel = 0;
    public GameObject mainPanel;

    public void Start()
    {

        instance = this;
        gameObject.SetActive(false);
    }

    public void Open(int playerIndex)
    {
        pausedIndex = playerIndex;
        EventSystemManager.instance.GetEventSystem(pausedIndex).SetSelectedGameObject(defaultObject);
        defaultObject.GetComponent<Button>().OnSelect(null);
        gameObject.SetActive(true);
        activePanel = 0;
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

        CrewManager.instance.DropPlayer(pausedIndex);
        //Close();
    }

    public void OpenOptionsMenu()
    {
        OptionsMenu.instance.Open();
        mainPanel.SetActive(false);
    }

    public void OpenMainMenu()
    {
        mainPanel.SetActive(true);
        EventSystemManager.instance.GetEventSystem(pausedIndex).SetSelectedGameObject(defaultObject);
        defaultObject.GetComponent<Button>().OnSelect(null);
    }

    public void SetActivePanel (int index)
    {
        foreach(GameObject panel in menuPanels)
        {
            panel.SetActive(false);
        }

        menuPanels[index].SetActive(true);
    }
}
