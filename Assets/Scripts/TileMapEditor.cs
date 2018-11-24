using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.UI;

public class TileMapEditor : MonoBehaviour
{

    public Color[] colors;
    public Camera gameCamera;

    public Map mMap;

    public Text tilePreview;
    private Color activeColor;

    public TileType mPlacedTileType;

    int lastMouseTileX = -1;
    int lastMouseTileY = -1;

    void Awake()
    {
        tilePreview.text = "Tiletype: " + mPlacedTileType.ToString();

        //SelectColor(0);
    }

    void Update()
    {
        
            HandleInput();
        
    }

    
    void HandleInput()
    {

        Vector2 mousePos = Input.mousePosition;
        Vector2 cameraPos = Camera.main.transform.position;
        var mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        int mouseTileX, mouseTileY;
        mMap.GetMapTileAtPoint(mousePosInWorld, out mouseTileX, out mouseTileY);

        if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (mouseTileX != lastMouseTileX || mouseTileY != lastMouseTileY || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Mouse2))
            {
                if (!mMap.IsNotEmpty(mouseTileX, mouseTileY))
                    mMap.SetTile(mouseTileX, mouseTileY, mPlacedTileType);
                else
                    mMap.SetTile(mouseTileX, mouseTileY, TileType.Empty);

                lastMouseTileX = mouseTileX;
                lastMouseTileY = mouseTileY;

                //Debug.Log(mouseTileX + "  " + mouseTileY);
            }
        }

        var wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel > 0.05f)
        {

            if ((int)mPlacedTileType < (int)TileType.Count - 1)
                mPlacedTileType += 1;
            else
                mPlacedTileType = TileType.Block;

            tilePreview.text = "Tiletype: " + mPlacedTileType.ToString();
        }
        else if (wheel < -0.05f)
        {
            if ((int)mPlacedTileType > (int)TileType.Empty + 1)
                mPlacedTileType -= 1;
            else
                mPlacedTileType = TileType.Count - 1;

            tilePreview.text = "Tiletype: " + mPlacedTileType.ToString();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
           //Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //Load();
        }

    }

   
   
}