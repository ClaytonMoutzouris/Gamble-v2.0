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
    public bool isOpen = false;
    public int panelHeight;

    // Start is called before the first frame update
    void Start()
    {
        //transform.localPosition -= new Vector3(0, panelHeight, 0);
        foreach (GameObject tab in tabs)
        {
            tab.SetActive(false);
        }

        tabs[selectedTabIndex].SetActive(true);
        

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

    public void PositionTab()
    {

    }

    public void OpenPlayerPanel()
    {
        EventSystemManager.instance.GetEventSystem(playerIndex).SetSelectedGameObject(inputAnchor);
        isOpen = true;
        transform.localPosition = transform.localPosition + new Vector3(0, panelHeight, 0);
    }


    public void ClosePlayerPanel()
    {
        EventSystemManager.instance.GetEventSystem(playerIndex).SetSelectedGameObject(null);
        transform.localPosition = transform.localPosition - new Vector3(0, panelHeight, 0);
        isOpen = false;

    }
}
