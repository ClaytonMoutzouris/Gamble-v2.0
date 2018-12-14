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
    public MapManager mMap;

    public float smoothTime = 0.15f;
    public Vector2 PlayerDistance;
    //public float smoothTimeX = 0.3f;
    private Vector3 velocity = Vector3.zero;
    public bool bounds;

    public float halfWidth;
    public float halfHeight;
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


        PlayerDistance = new Vector2(Mathf.Abs(mPlayer1.position.x - mPlayer2.position.x), Mathf.Abs(mPlayer1.position.y - mPlayer2.position.y));
        targetPos = (mPlayer1.position + mPlayer2.position) *0.5f;

        var cameraPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        //Debug.Log("Camera position " + cameraPos);

        halfHeight = camera.orthographicSize;
        halfWidth = camera.aspect * halfHeight;

        //If the players are farther apart than the camera is wide, expand the camera
        if (PlayerDistance.x > halfWidth*2)
        {
            camera.orthographicSize = camera.orthographicSize * (PlayerDistance.x / (halfWidth * 2));
        }

        //If the players are farther apart than the camera is tall, expand the camera
        if (PlayerDistance.y > halfHeight * 2)
        {
            camera.orthographicSize = camera.orthographicSize * (PlayerDistance.y / (halfHeight * 2));
        }

        //Keep the camera within the bounds of the maps width
        if (cameraPos.x - halfWidth + MapManager.cTileSize / 2 < 0)
        {
            cameraPos.x = halfWidth - MapManager.cTileSize / 2;
        } else if(cameraPos.x + halfWidth + MapManager.cTileSize / 2 > mMap.mWidth * MapManager.cTileSize)
        {
            cameraPos.x = mMap.mWidth * MapManager.cTileSize - (halfWidth + MapManager.cTileSize / 2);
        }

        //Keep the camera within the bounds of the maps height
        if (cameraPos.y - halfHeight + MapManager.cTileSize/2 < 0)
        {
            cameraPos.y = halfHeight - MapManager.cTileSize / 2;
        }
        else if (cameraPos.y + halfHeight + MapManager.cTileSize / 2 > mMap.mHeight * MapManager.cTileSize)
        {
            cameraPos.y = mMap.mHeight * MapManager.cTileSize - (halfHeight + MapManager.cTileSize / 2);
        }

        transform.position = new Vector3(cameraPos.x, cameraPos.y, transform.position.z);
        
    }
}
