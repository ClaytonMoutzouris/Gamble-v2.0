using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEditorDropdown : MonoBehaviour
{
    public GameObject dropdownOptions;
    public bool open = false;

    // Update is called once per frame
    void Update()
    {
        if(open)
        {
            if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
            {
                OpenCloseDropdown();

            }
        }


    }

    public void OpenCloseDropdown()
    {

        dropdownOptions.SetActive(!open);
        open = !open;

    }





}
