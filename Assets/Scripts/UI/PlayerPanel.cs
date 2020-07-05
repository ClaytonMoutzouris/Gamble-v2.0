using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum PlayerPanelTabType { Inventory, Stats, Ship, Level };

public class PlayerPanel : MonoBehaviour
{
    public int playerIndex;
    public PlayerPanelTooltip tooltip;
    public HealthBar healthBar;
    public PlayerInventoryUI inventoryUI;
    public PlayerInfoUI uiPlayerTab;
    public PlayerLevelTabUI talentTree;
    public PlayerPanelTabType selectedTabIndex = PlayerPanelTabType.Inventory;
    public List<PlayerPanelTab> tabs;
    public GameObject inputAnchor;
    public GameObject menuObject;
    public bool isOpen = false;
    public int panelHeight;

    // Start is called before the first frame update
    public void Initialize()
    {
        //transform.localPosition -= new Vector3(0, panelHeight, 0);
        foreach (PlayerPanelTab tab in tabs)
        {
            if(tab != null)
            {
                tab.Close();
            }
        }

        tabs[(int)selectedTabIndex].Open();
        inventoryUI.Initialize();
        menuObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTab(int index)
    {

        tabs[(int)selectedTabIndex].Close();
        tabs[index].Open();
        selectedTabIndex = (PlayerPanelTabType)index;
    }

    public void NextTabRight()
    {
        tabs[(int)selectedTabIndex].Close();

        selectedTabIndex++;
        if((int)selectedTabIndex >= tabs.Count)
        {
            selectedTabIndex = 0;
        }

        tabs[(int)selectedTabIndex].Open();

    }

    public void NextTabLeft()
    {
        tabs[(int)selectedTabIndex].Close();

        selectedTabIndex--;
        if (selectedTabIndex < 0)
        {
            selectedTabIndex = (PlayerPanelTabType)tabs.Count-1;
        }

        tabs[(int)selectedTabIndex].Open();
    }

    public void PositionTab()
    {

    }

    public void OpenPlayerPanel()
    {
        SetTab(0);
        EventSystemManager.instance.GetEventSystem(playerIndex).SetSelectedGameObject(inputAnchor);
        inputAnchor.GetComponent<Button>().OnSelect(null);
        isOpen = true;
        transform.localPosition = transform.localPosition + new Vector3(0, panelHeight, 0);
        tooltip.gameObject.SetActive(true);

        GameObject tabHeaderUI = GameObject.Find("TabHeaderContainer");
        GameObject tabsUI = GameObject.Find("Tabs");
        menuObject.SetActive(true);
    }


    public void ClosePlayerPanel()
    {
        foreach (PlayerPanelTab tab in tabs)
        {
            if (tab != null)
            {
                tab.Close();
            }
        }
        EventSystemManager.instance.GetEventSystem(playerIndex).SetSelectedGameObject(null);
        transform.localPosition = transform.localPosition - new Vector3(0, panelHeight, 0);
        isOpen = false;
        menuObject.SetActive(false);
        tooltip.SetTooptip("");
        tooltip.gameObject.SetActive(false);
    }

    #region deprecated
    public void HideUI(bool hide)
    {
        GameObject tabHeaderUI = GameObject.Find("TabHeaderContainer");
        GameObject tabsUI = GameObject.Find("Tabs");

        if (hide)
        {
            inventoryUI.GetComponent<CanvasGroup>().alpha = 0;
            uiPlayerTab.GetComponent<CanvasGroup>().alpha = 0;
            tooltip.GetComponent<CanvasGroup>().alpha = 0;
            tabHeaderUI.GetComponent<CanvasGroup>().alpha = 0;
            tabsUI.GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            inventoryUI.GetComponent<CanvasGroup>().alpha = 1;
            uiPlayerTab.GetComponent<CanvasGroup>().alpha = 1;
            tooltip.GetComponent<CanvasGroup>().alpha = 1;
            tabHeaderUI.GetComponent<CanvasGroup>().alpha = 1;
            tabsUI.GetComponent<CanvasGroup>().alpha = 1;
        }


    }
    #endregion
}
