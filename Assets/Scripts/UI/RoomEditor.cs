using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

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

    public Dropdown roomTypeDropdown;
    public Dropdown mapLayerDropdown;
    public Dropdown worldTypeDropdown;


    public Text tilePreview;
    private Color activeColor;
    public EditorMode mode;
    public TileType mPlacedTileType;
    public List<GameObject> objectPrefabs;
    public int mObjectIndex = 0;

    int lastMouseTileX = -1;
    int lastMouseTileY = -1;

    void Awake()
    {
        tilePreview.text = "Tiletype: " + mPlacedTileType.ToString();
        //SelectColor(0);
        List<string> edgeOptions = new List<string>();

        for(int i = 0; i < (int)RoomType.Count; i++)
        {
            edgeOptions.Add(((RoomType)i).ToString());
        }
        roomTypeDropdown.AddOptions(edgeOptions);

        edgeOptions.Clear();

        for (int i = 0; i < (int)SurfaceLayer.Count; i++)
        {
            edgeOptions.Add(((SurfaceLayer)i).ToString());
        }
        mapLayerDropdown.AddOptions(edgeOptions);

        edgeOptions.Clear();

        for (int i = 0; i < (int)WorldType.Count; i++)
        {
            edgeOptions.Add(((WorldType)i).ToString());
        }
        worldTypeDropdown.AddOptions(edgeOptions);

    }

    void Update()
    {

        HandleInput();

    }


    public void UpdateEditorValues()
    {
        roomTypeDropdown.value = (int)mMap.room.roomType;
        worldTypeDropdown.value = (int)mMap.room.worldType;
        mapLayerDropdown.value = (int)mMap.room.surfaceLayer;
        //SetChunkEdgeType();
    }

    public void ChangeMode(int index)
    {
        mode = (EditorMode)index;
    }


    public void ChangeRoomType(int index)
    {
        mMap.room.roomType = (RoomType)index;
    }

    public void ChangeWorldType(int index)
    {
        mMap.room.worldType = (WorldType)index;
    }

    public void ChangeMapLayer(int index)
    {
        mMap.room.surfaceLayer = (SurfaceLayer)index;
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
        Vector2 mousePos = mMap.GetMapTileAtPoint(mousePosInWorld);
        mouseTileX = (int)mousePos.x;
        mouseTileY = (int)mousePos.y;

        if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {


            mMap.SetTile(mouseTileX, mouseTileY, mPlacedTileType);

        }

        var wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel > 0.05f)
        {

            if ((int)mPlacedTileType < (int)TileType.Count)
            {
                mPlacedTileType += 1;
            }
            else
            {
                mPlacedTileType = TileType.Empty;
            }

            tilePreview.text = "Tiletype: " + mPlacedTileType.ToString();
        }
        else if (wheel < -0.05f)
        {
            if ((int)mPlacedTileType > (int)TileType.Empty)
                mPlacedTileType -= 1;
            else
                mPlacedTileType = TileType.Count-1;

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

            GameObject temp = Instantiate(objectPrefabs[mObjectIndex]);

            temp.transform.position = mMap.GetMapTilePosition(new Vector2i(mouseTileX, mouseTileY));

        }

        var wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel > 0.05f)
        {

            if ((int)mObjectIndex < (int)objectPrefabs.Count - 1)
                mObjectIndex += 1;
            else
                mObjectIndex = 0;

            tilePreview.text = "Tiletype: " + objectPrefabs[mObjectIndex].name;
        }
        else if (wheel < -0.05f)
        {
            if ((int)mObjectIndex > 0)
                mObjectIndex -= 1;
            else
                mObjectIndex = objectPrefabs.Count - 1;

            tilePreview.text = "Tiletype: " + objectPrefabs[mObjectIndex].name;
        }

    }

    void EnemyMode()
    {

    }

    void MiscMode()
    {

    }


}