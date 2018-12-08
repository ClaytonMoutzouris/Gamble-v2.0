using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.UI;

public enum EditorMode
{
    Tiles,
    Objects,
    Enemies,
    Misc
}

public class RoomEditor : MonoBehaviour
{

    public Color[] colors;
    public Camera gameCamera;

    public EditorMap mMap;

    public Text tilePreview;
    private Color activeColor;
    public EditorMode mode;
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

    public void ChangeMode(int index)
    {
        mode = (EditorMode)index;
    }


    void HandleInput()
    {

        //Vector2 mousePos = Input.mousePosition;
        //Vector2 cameraPos = Camera.main.transform.position;


        switch (mode)
        {
            case EditorMode.Tiles:
                TileMode();
                break;
            case EditorMode.Objects:
                ObjectMode();
                break;
            case EditorMode.Enemies:
                EnemyMode();
                break;
            case EditorMode.Misc:
                MiscMode();
                break;

        }
        

    }

    void TileMode()
    {
        var mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int mouseTileX, mouseTileY;
        mMap.GetMapTileAtPoint(mousePosInWorld, out mouseTileX, out mouseTileY);

        if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {


            mMap.SetTile(mouseTileX, mouseTileY, mPlacedTileType);

        }

        var wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel > 0.05f)
        {

            if ((int)mPlacedTileType < (int)TileType.Count - 1)
                mPlacedTileType += 1;
            else
                mPlacedTileType = TileType.Empty;

            tilePreview.text = "Tiletype: " + mPlacedTileType.ToString();
        }
        else if (wheel < -0.05f)
        {
            if ((int)mPlacedTileType > (int)TileType.Empty)
                mPlacedTileType -= 1;
            else
                mPlacedTileType = TileType.Count - 1;

            tilePreview.text = "Tiletype: " + mPlacedTileType.ToString();
        }
    }

    void ObjectMode()
    {
        var mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int mouseTileX, mouseTileY;
        mMap.GetMapTileAtPoint(mousePosInWorld, out mouseTileX, out mouseTileY);

        if (Input.GetKeyDown(KeyCode.Mouse1) && !EventSystem.current.IsPointerOverGameObject())
        {

            MovingPlatform temp =Instantiate(Resources.Load<MovingPlatform>("Prefabs/MovingPlatform"));
            temp.Init();
            temp.SetTilePosition(new Vector2i(mouseTileX, mouseTileY));

        }

       
    }

    void EnemyMode()
    {

    }

    void MiscMode()
    {

    }


}