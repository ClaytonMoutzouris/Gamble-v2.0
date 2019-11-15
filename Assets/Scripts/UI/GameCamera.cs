using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCamera : MonoBehaviour
{
    public static GameCamera instance;
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
    public MapManager mMap;

    public float mZoomSpeed = 10f;
    public float mMinOrthographicSize = 120;
    public float mBoundingBoxPadding = 32;

    void Awake()
    {
        instance = this;
        mCamera = GetComponent<Camera>();
        
        //mCamera.orthographicSize = mMinOrthographicSize;
        //mPosition = transform.position;
    }

    public void LateUpdate()
    {
        if(mMap.mCurrentMap.mapType == MapType.Hub)
        {
            mMinOrthographicSize = 80;
        } else
        {
            mMinOrthographicSize = 120;
        }

        Rect boundingBox = CalculateTargetsBoundingBox();
        
        transform.position = CalculateCameraPosition(boundingBox);
        mCamera.orthographicSize = CalculateOrthographicSize(boundingBox);
        StayWithinBounds();
    }

    void StayWithinBounds()
    {
        var cameraPos = transform.position;
        var halfHeight = mCamera.orthographicSize;
        var halfWidth = mCamera.aspect * halfHeight;
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

    Rect CalculateTargetsBoundingBox()
    {
        float minX = mMap.mWidth * MapManager.cTileSize;
        float maxX = 0;
        float minY = mMap.mHeight * MapManager.cTileSize;
        float maxY = 0;

        foreach (Player player in LevelManager.instance.players)
        {
            if (player == null || player.IsDead)
                continue;

            Vector3 position = player.Position;

            minX = Mathf.Min(minX, position.x);
            minY = Mathf.Min(minY, position.y);
            maxX = Mathf.Max(maxX, position.x);
            maxY = Mathf.Max(maxY, position.y);
        }


        return Rect.MinMaxRect(minX - mBoundingBoxPadding, maxY + mBoundingBoxPadding, maxX + mBoundingBoxPadding, minY - mBoundingBoxPadding);
    }

    float CalculateOrthographicSize(Rect boundingBox)
    {
        float orthographicSize = mCamera.orthographicSize;
        Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, boundingBox.y, 0f);
        Vector3 topRightAsViewport = mCamera.WorldToViewportPoint(topRight);

        if (topRightAsViewport.x >= topRightAsViewport.y)
            orthographicSize = Mathf.Abs(boundingBox.width) / mCamera.aspect / 2f;
        else
            orthographicSize = Mathf.Abs(boundingBox.height) / 2f;

        return Mathf.Clamp(Mathf.Lerp(mCamera.orthographicSize, orthographicSize, Time.deltaTime * mZoomSpeed), mMinOrthographicSize, Mathf.Infinity);
    }

    Vector3 CalculateCameraPosition(Rect boundingBox)
    {
        Vector2 boundingBoxCenter = boundingBox.center;

        return new Vector3(boundingBoxCenter.x, boundingBoxCenter.y, mCamera.transform.position.z);
    }

    /* Old Code
    void UpdateZoom()
    {
        halfHeight = mCamera.orthographicSize;
        halfWidth = mCamera.aspect * halfHeight;
        float tempSize = mCamera.orthographicSize;
        bool growY = false, growX = false;
        bool shrinkY = false, shrinkX = false;

        //First try to expand
        if (PlayerDistance.x > (halfWidth*2) - EdgeBuffer.x)
        {
            growX = true;
        }

        if ((halfWidth * 2) >= PlayerDistance.x + EdgeBuffer.x)
        {
            shrinkX = true;
        }

        
        //If the players are farther apart than the camera is tall, expand the camera
        if (PlayerDistance.y > (halfHeight*2) - EdgeBuffer.y)
        {
            growY = true;
        }

        
        if ((halfHeight * 2) >= PlayerDistance.y + EdgeBuffer.y)
        {
            shrinkY = true;
        }



        if (growY || shrinkY)
        {
            tempSize = tempSize * ((PlayerDistance.y + EdgeBuffer.y) / (halfHeight * 2));
        }

        if (growX || shrinkX)
        {
            tempSize = tempSize * ((PlayerDistance.x + EdgeBuffer.x) / (halfWidth * 2));
        }

        if (tempSize < mMinOrthographicSize)
            tempSize = mMinOrthographicSize;

        mCamera.orthographicSize = tempSize;
    }

    void CameraFollow()
    {
        targetPos = (mPlayer1.position + mPlayer2.position) * 0.5f;

        var cameraPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        //Debug.Log("Camera position " + cameraPos);



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
    */
}
