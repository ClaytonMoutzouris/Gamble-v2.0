using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public Transform camera;
    Vector3 lastpos;
    public List<BackgroundLayer> backgroundLayers;

    void Start()
    {
        camera = Camera.main.transform;
        lastpos = camera.position;
    }

    private void Update()
    {
        foreach(BackgroundLayer layer in backgroundLayers)
        {
            layer.transform.position -= ((lastpos - camera.position) * layer.speedCoefficient);

        }
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
