using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalCoop;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour {

    public static PauseMenu instance;
    public GameObject defaultObject;

    public void Start()
    {

        instance = this;
        gameObject.SetActive(false);
    }

    public void Open()
    {
        EventSystem.current.SetSelectedGameObject(defaultObject);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

}
