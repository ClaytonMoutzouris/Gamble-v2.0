using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerPanel : MonoBehaviour
{
    public int playerIndex;
    public PlayerPanelTooltip tooltip;
    public HealthBar healthBar;
    public PlayerInventoryUI inventoryUI;
    public UIPlayerTab uiPlayerTab;
    public int selectedTabIndex = 0;
    public List<GameObject> tabs;
    public GameObject inputAnchor;
    public GameObject menuObject;
    public bool isOpen = false;
    public int panelHeight;

    // Start is called before the first frame update
    public void Initialize()
    {
        //transform.localPosition -= new Vector3(0, panelHeight, 0);
        foreach (GameObject tab in tabs)
        {
            tab.SetActive(false);
        }

        tabs[selectedTabIndex].SetActive(true);
        inventoryUI.Initialize();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTab(int index)
    {
        if(selectedTabIndex == index)
        {
            return;
        }

        tabs[selectedTabIndex].SetActive(false);
        tabs[index].SetActive(true);
        selectedTabIndex = index;
    }

    public void NextTabRight()
    {
        tabs[selectedTabIndex].SetActive(false);

        selectedTabIndex++;
        if(selectedTabIndex >= tabs.Count)
        {
            selectedTabIndex = 0;
        }

        tabs[selectedTabIndex].SetActive(true);

    }

    public void NextTabLeft()
    {
        tabs[selectedTabIndex].SetActive(false);

        selectedTabIndex--;
        if (selectedTabIndex < 0)
        {
            selectedTabIndex = tabs.Count-1;
        }

        tabs[selectedTabIndex].SetActive(true);
    }

    public void PositionTab()
    {

    }

    public void OpenPlayerPanel()
    {
        EventSystemManager.instance.GetEventSystem(playerIndex).SetSelectedGameObject(inputAnchor);
        isOpen = true;
        transform.localPosition = transform.localPosition + new Vector3(0, panelHeight, 0);

        GameObject tabHeaderUI = GameObject.Find("TabHeaderContainer");
        GameObject tabsUI = GameObject.Find("Tabs");
        menuObject.SetActive(true);
    }


    public void ClosePlayerPanel()
    {
        EventSystemManager.instance.GetEventSystem(playerIndex).SetSelectedGameObject(null);
        transform.localPosition = transform.localPosition - new Vector3(0, panelHeight, 0);
        isOpen = false;
        menuObject.SetActive(false);
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
