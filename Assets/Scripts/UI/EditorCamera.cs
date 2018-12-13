using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;

public class EditorCamera : MonoBehaviour
{


    /// <summary>
    /// The position.
    /// </summary>
    public Vector3 mPosition;
    public Vector2 targetPos;
    /// <summary>
    /// The map. Assigned from editor.
    /// </summary>
    public EditorMap mMap;
    Camera camera;

    public float smoothTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public float MinZoom, stiMaxZoom;

    const int cOuterVisibilityX = 0;
    const int cOuterVisibilityY = 0;
    float zoom = 1f;
    public float mCameraMoveSpeed = 25;



    void Start()
    {
        //transform.position = new Vector3((mMap.mWidth*Map.cTileSize) / 2, (mMap.mHeight*Map.cTileSize) / 2, -10);
        camera = GetComponent<Camera>();
    }


    public void FixedUpdate()
    {






        var cameraPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        //Debug.Log("Camera position " + cameraPos);

        float halfHeight = camera.orthographicSize;
        float halfWidth = camera.aspect * halfHeight;

        if(halfWidth * 2 > mMap.mWidth * MapManager.cTileSize)
        {
            camera.orthographicSize = camera.orthographicSize *( (mMap.mWidth*MapManager.cTileSize) / (halfWidth * 2));
        }

        //Keep the camera within the bounds of the maps width
        if (cameraPos.x - halfWidth + MapManager.cTileSize / 2 < 0)
        {
            cameraPos.x = halfWidth - MapManager.cTileSize / 2;
        }
        else if (cameraPos.x + halfWidth + MapManager.cTileSize / 2 > mMap.mWidth * MapManager.cTileSize)
        {
            cameraPos.x = mMap.mWidth * MapManager.cTileSize - (halfWidth + MapManager.cTileSize / 2);
        }

        //Keep the camera within the bounds of the maps height
        if (cameraPos.y - halfHeight + MapManager.cTileSize / 2 < 0)
        {
            cameraPos.y = halfHeight - MapManager.cTileSize / 2;
        }
        else if (cameraPos.y + halfHeight + MapManager.cTileSize / 2 > mMap.mHeight * MapManager.cTileSize)
        {
            cameraPos.y = mMap.mHeight * MapManager.cTileSize - (halfHeight + MapManager.cTileSize / 2);
        }

        transform.position = new Vector3(cameraPos.x, cameraPos.y, transform.position.z);

    }

    public void Update()
    {
        
        float zDelta = Input.GetAxis("Vertical");

        if ( zDelta != 0f)
        {
            AdjustPosition( zDelta);
        }
        
        //Debug.Log(Camera.main.pixelWidth + " " + Camera.main.pixelHeight);
        //camera.rect = Rect((Screen.width - 512.0) / Screen.width, (Screen.height - 256.0) / Screen.height, 384.0 / Screen.width, 96.0 / Screen.height);
    }

    public void AdjustPosition( float y)
    {
        targetPos = transform.position + new Vector3(0, y * mCameraMoveSpeed);

    }

}
