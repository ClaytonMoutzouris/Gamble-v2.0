﻿using UnityEngine;
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
    NPC,
    Enemies,
    Misc,
}

public class RoomEditor : MonoBehaviour
{

    public Color[] colors;
    public Camera gameCamera;

    public EditorMap mMap;

    public Dropdown roomTypeDropdown;
    public Dropdown mapLayerDropdown;
    public Dropdown worldTypeDropdown;

    //FeatureDropdowns
    public List<Dropdown> featureDropdowns;
    public Dropdown tileTypeDropdown;
    public Dropdown objectTypeDropdown;
    public Dropdown npcTypeDropdown;

    public Text tilePreview;
    private Color activeColor;
    public EditorMode mode;
    public TileType mPlacedTileType;
    public ObjectType mPlaceObjectType;
    public NPCType mPlaceNPCType;

    public int mObjectIndex = 0;

    int lastMouseTileX = -1;
    int lastMouseTileY = -1;

    public GameObject selectorIndicator;

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

        edgeOptions.Clear();

        for (int i = 0; i < (int)TileType.Count; i++)
        {
            edgeOptions.Add(((TileType)i).ToString());
        }
        tileTypeDropdown.AddOptions(edgeOptions);

        edgeOptions.Clear();

        for (int i = 0; i < (int)ObjectType.Count; i++)
        {
            edgeOptions.Add(((ObjectType)i).ToString());
        }
        objectTypeDropdown.AddOptions(edgeOptions);

        edgeOptions.Clear();
        for (int i = 0; i < (int)NPCType.Count; i++)
        {
            edgeOptions.Add(((NPCType)i).ToString());
        }
        npcTypeDropdown.AddOptions(edgeOptions);
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

        switch(mode)
        {
            case EditorMode.Tiles:
                objectTypeDropdown.gameObject.SetActive(false);
                npcTypeDropdown.gameObject.SetActive(false);
                tileTypeDropdown.gameObject.SetActive(true);
                break;
            case EditorMode.Objects:
                objectTypeDropdown.gameObject.SetActive(true);
                npcTypeDropdown.gameObject.SetActive(false);
                tileTypeDropdown.gameObject.SetActive(false);
                break;
            case EditorMode.NPC:
                objectTypeDropdown.gameObject.SetActive(false);
                tileTypeDropdown.gameObject.SetActive(false);
                npcTypeDropdown.gameObject.SetActive(true);

                break;
        }
    }

    public void ChangeTiletype(int index)
    {
        mPlacedTileType = (TileType)index;
    }

    public void ChangeObjecttype(int index)
    {
        mPlaceObjectType = (ObjectType)index;
    }

    public void ChangeNPCtype(int index)
    {
        mPlaceNPCType = (NPCType)index;
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
            case EditorMode.NPC:
                NPCMode();
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

        if(mouseTileX < 0 || mouseTileX >= mMap.mWidth || mouseTileY < 0 || mouseTileY >= mMap.mHeight)
        {
            return;
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            EditorCamera.instance.SetTargetTile(mouseTileX, mouseTileY);

            selectorIndicator.transform.position = mMap.GetMapTilePosition(mouseTileX, mouseTileY);
        }


        if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {

            mMap.SetTile(mouseTileX, mouseTileY, mPlacedTileType);

        }

        
    }

    void ObjectMode()
    {
        var mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int mouseTileX, mouseTileY;
        Vector2 mousePos = mMap.GetMapTileAtPoint(mousePosInWorld);
        mouseTileX = (int)mousePos.x;
        mouseTileY = (int)mousePos.y;

        if (mouseTileX < 0 || mouseTileX >= mMap.mWidth || mouseTileY < 0 || mouseTileY >= mMap.mHeight)
        {
            return;
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            EditorCamera.instance.SetTargetTile(mouseTileX, mouseTileY);

            selectorIndicator.transform.position = mMap.GetMapTilePosition(mouseTileX, mouseTileY);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //GameObject temp = Instantiate(objectPrefabs[mObjectIndex]);
            //temp.transform.position = mMap.GetMapTilePosition(new Vector2i(mouseTileX, mouseTileY));
            mMap.AddObjectEntity(new ObjectData(mouseTileX, mouseTileY, mPlaceObjectType));
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !EventSystem.current.IsPointerOverGameObject())
        {
            //GameObject temp = Instantiate(objectPrefabs[mObjectIndex]);
            //temp.transform.position = mMap.GetMapTilePosition(new Vector2i(mouseTileX, mouseTileY));
            mMap.ClearObjectEntity(mouseTileX, mouseTileY);
        }

    }

    void NPCMode()
    {
        var mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int mouseTileX, mouseTileY;
        Vector2 mousePos = mMap.GetMapTileAtPoint(mousePosInWorld);
        mouseTileX = (int)mousePos.x;
        mouseTileY = (int)mousePos.y;

        if (mouseTileX < 0 || mouseTileX >= mMap.mWidth || mouseTileY < 0 || mouseTileY >= mMap.mHeight)
        {
            return;
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            EditorCamera.instance.SetTargetTile(mouseTileX, mouseTileY);

            selectorIndicator.transform.position = mMap.GetMapTilePosition(mouseTileX, mouseTileY);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //GameObject temp = Instantiate(objectPrefabs[mObjectIndex]);
            //temp.transform.position = mMap.GetMapTilePosition(new Vector2i(mouseTileX, mouseTileY));
            mMap.AddNPCEntity(new NPCData(mouseTileX, mouseTileY, mPlaceNPCType));
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !EventSystem.current.IsPointerOverGameObject())
        {
            //GameObject temp = Instantiate(objectPrefabs[mObjectIndex]);
            //temp.transform.position = mMap.GetMapTilePosition(new Vector2i(mouseTileX, mouseTileY));
            mMap.ClearObjectEntity(mouseTileX, mouseTileY);
        }

    }


    void EnemyMode()
    {

    }

    void MiscMode()
    {

    }


}