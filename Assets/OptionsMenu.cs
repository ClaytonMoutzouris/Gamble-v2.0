using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu instance;
    public GameObject defaultObject;


    public void UpdateFieldOfView(float value)
    {
        GameCamera.instance.mMinOrthographicSize = value;
    }

    public void Start()
    {
        instance = this;
        gameObject.SetActive(false);

    }

    public void ReturnToMainMenu()
    {
        PauseMenu.instance.OpenMainMenu();
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        GamepadInputManager.instance.gamepadInputs[PauseMenu.instance.pausedIndex].GetEventSystem().SetSelectedGameObject(defaultObject);
        //defaultObject.GetComponent<Button>().OnSelect(null);

    }

}
