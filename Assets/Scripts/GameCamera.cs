using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour
{

    /// <summary>
    /// A reference to the the player.
    /// </summary>
    public Transform mPlayer1;
    public Transform mPlayer2;

    /// <summary>
    /// The position.
    /// </summary>
    public Vector3 mPosition;
    public Vector3 targetPos;
    Camera camera;
    /// <summary>
    /// The map. Assigned from editor.
    /// </summary>
    public Map mMap;

    public float smoothTime = 0.15f;
    //public float smoothTimeX = 0.3f;
    private Vector3 velocity = Vector3.zero;
    public bool bounds;


    const int cOuterVisibilityX = 0;
    const int cOuterVisibilityY = 0;

    void Start()
    {
        camera = GetComponent<Camera>();

        //mPosition = transform.position;
    }

    public void FixedUpdate()
    {

        if (mPlayer1 == null || mPlayer2 == null)
            return;

        

        targetPos = (mPlayer1.position + mPlayer2.position) *0.5f;

        var cameraPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        //Debug.Log("Camera position " + cameraPos);

        float halfHeight = camera.orthographicSize;
        float halfWidth = camera.aspect * halfHeight;

        //Keep the camera within the bounds of the maps width
        if(cameraPos.x - halfWidth + Map.cTileSize / 2 < 0)
        {
            cameraPos.x = halfWidth - Map.cTileSize / 2;
        } else if(cameraPos.x + halfWidth + Map.cTileSize / 2 > mMap.mWidth * Map.cTileSize)
        {
            cameraPos.x = mMap.mWidth * Map.cTileSize - (halfWidth + Map.cTileSize / 2);
        }

        //Keep the camera within the bounds of the maps height
        if (cameraPos.y - halfHeight + Map.cTileSize/2 < 0)
        {
            cameraPos.y = halfHeight - Map.cTileSize / 2;
        }
        else if (cameraPos.y + halfHeight + Map.cTileSize / 2 > mMap.mHeight * Map.cTileSize)
        {
            cameraPos.y = mMap.mHeight * Map.cTileSize - (halfHeight + Map.cTileSize / 2);
        }

        transform.position = new Vector3(cameraPos.x, cameraPos.y, transform.position.z);
        
    }
}
