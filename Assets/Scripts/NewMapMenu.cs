using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMapMenu : MonoBehaviour
{

    public Map mMap;

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}