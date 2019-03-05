using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerPanel : MonoBehaviour
{
    public HealthBar healthBar;
    public PlayerInventoryUI inventoryUI;
    public int selectedTabIndex = 0;
    public List<GameObject> tabs;
    public GameObject inputAnchor;
    public bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject tab in tabs)
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
        EventSystem.current.SetSelectedGameObject(inputAnchor);
        isOpen = true;
        transform.localPosition = transform.localPosition + new Vector3(0, 230, 0);
    }


    public void ClosePlayerPanel()
    {
        transform.localPosition = transform.localPosition - new Vector3(0, 230, 0);
        isOpen = false;

    }
}
