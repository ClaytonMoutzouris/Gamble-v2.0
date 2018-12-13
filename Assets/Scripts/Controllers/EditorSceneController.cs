using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorSceneController : MonoBehaviour {

    public static EditorSceneController instance;
    public Camera gameCamera;

    public EditorMap mMap;

    void Start()
    {
        Application.targetFrameRate = 60;

        //player2.GetComponent<SpriteRenderer>().color = Color.gray;

        NewMap();

    }

    public void NewMap()
    {
        mMap.Init();
    }
}
