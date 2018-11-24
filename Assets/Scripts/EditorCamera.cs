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
    Vector2 targetPos;
    /// <summary>
    /// The map. Assigned from editor.
    /// </summary>
    public EditorMap mMap;

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public float MinZoom, stiMaxZoom;

    const int cOuterVisibilityX = 0;
    const int cOuterVisibilityY = 0;
    float zoom = 1f;
    public float mCameraMoveSpeed = 25;



    void Start()
    {
        transform.position = new Vector3((mMap.mWidth*Map.cTileSize) / 2, (mMap.mHeight*Map.cTileSize) / 2, -10);
        mPosition = transform.position;
    }

    
    public void LateUpdate()
    {


        

        

        mPosition = new Vector3(targetPos.x, targetPos.y, mPosition.z);

        var point = GetComponent<Camera>().WorldToViewportPoint(mPosition);
        var delta = mPosition - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        var destination = transform.position + delta;

        var cameraPos = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        if (Mathf.Abs(cameraPos.x - targetPos.x) < 2.0f)
            cameraPos.x = targetPos.x;
        if (Mathf.Abs(cameraPos.y - targetPos.y) < 2.0f)
            cameraPos.y = targetPos.y;

        //make sure the camera doesn't go outside the map bounds on x axis
        if (cameraPos.x < mMap.mPosition.x + Camera.main.pixelWidth * 0.5f - Map.cTileSize / 2 + cOuterVisibilityX * Map.cTileSize)
            cameraPos.x = mMap.mPosition.x + Camera.main.pixelWidth * 0.5f - Map.cTileSize / 2 + cOuterVisibilityX * Map.cTileSize;
        else if (cameraPos.x > mMap.mPosition.x + mMap.mWidth * Map.cTileSize - Camera.main.pixelWidth * 0.5f - Map.cTileSize / 2 - cOuterVisibilityX * Map.cTileSize)
            cameraPos.x = mMap.mPosition.x + mMap.mWidth * Map.cTileSize - Camera.main.pixelWidth * 0.5f - Map.cTileSize / 2 - cOuterVisibilityX * Map.cTileSize;

        //make sure the camera doesn't go outside the map bounds on y axis
        if (cameraPos.y < mMap.mPosition.y + Camera.main.pixelHeight * 0.5f - Map.cTileSize / 2 + cOuterVisibilityY * Map.cTileSize)
            cameraPos.y = mMap.mPosition.y + Camera.main.pixelHeight * 0.5f - Map.cTileSize / 2 + cOuterVisibilityY * Map.cTileSize;
        else if (cameraPos.y > mMap.mPosition.y + mMap.mHeight * Map.cTileSize - Camera.main.pixelHeight * 0.5f - Map.cTileSize / 2 - cOuterVisibilityY * Map.cTileSize)
            cameraPos.y = mMap.mPosition.y + mMap.mHeight * Map.cTileSize - Camera.main.pixelHeight * 0.5f - Map.cTileSize / 2 - cOuterVisibilityY * Map.cTileSize;



        transform.position = new Vector3(Mathf.Round(cameraPos.x), Mathf.Round(cameraPos.y), cameraPos.z);
        mPosition = transform.position;

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
        targetPos = mPosition + new Vector3(0, y * mCameraMoveSpeed);

    }

}
