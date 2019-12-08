using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public Transform camera;
    public float speedCoefficient;
    Vector3 lastpos;

    void Start()
    {
        camera = Camera.main.transform;
        lastpos = camera.position;
    }

    private void Update()
    {
        transform.position -= ((lastpos - camera.position) * speedCoefficient);
        lastpos = camera.position;

        if(MapManager.instance.mCurrentMap.worldType == WorldType.Void)
        {
            transform.Rotate(0, 0, 0.05f);
        } else
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
