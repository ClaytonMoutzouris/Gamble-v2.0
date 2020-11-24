using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditorCamera : MonoBehaviour
{
    public static EditorCamera instance;
    /// <summary>
    /// A reference to the the player.
    /// </summary>

    /// <summary>
    /// The position.
    /// </summary>
    Camera mCamera;
    /// <summary>
    /// The map. Assigned from editor.
    /// </summary>
    public EditorMap mMap;

    public float mMinOrthographicSize = 120;

    Vector2i targetTile = new Vector2i(0,0);
    
    public Vector3 cameraOffset;

    void Awake()
    {
        instance = this;
        mCamera = GetComponent<Camera>();

        //mCamera.orthographicSize = mMinOrthographicSize;
        //mPosition = transform.position;
    }

    public void Update()
    {
        mCamera.orthographicSize = mMinOrthographicSize;
        transform.position = Vector3.Lerp(transform.position, (Vector3)EditorMap.instance.GetMapTilePosition(targetTile) + cameraOffset, .01f);
        UpdateZoom();

    }

    public void SetTargetTile(int x, int y)
    {
        if(Mathf.Abs(targetTile.x - x) >= 3 || Mathf.Abs(targetTile.y - y) >= 3)
            targetTile = new Vector2i(x, y);
    }

    public void UpdateZoom()
    {
        var wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel > 0.05f)
        {
            mMinOrthographicSize -= 5;
        }
        else if (wheel < -0.05f)
        {
            mMinOrthographicSize += 5;

        }
    }

}