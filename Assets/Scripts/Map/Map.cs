using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Map
{
    public Vector2i startTile = new Vector2i(1, 1);
    public Vector2i exitTile = new Vector2i(1, 1);
    public Vector2i bossTile = new Vector2i(1, 1);

    public WorldType worldType;
    public MapType mapType;
    public int sizeX;
    public int sizeY;
    public int roomSizeX;
    public int roomSizeY;

    MapData data;
    TileType[,] tiles;

    public float gravity = 800.0f;
    //public Audio

    public List<EntityData> objects;

    public MapData Data { get => data; set => data = value; }

    public Map(MapType mapType = MapType.Hub, WorldType type = WorldType.Forest, int sizex = 10, int sizey = 10)
    {
        this.mapType = mapType;
        this.worldType = type;
        sizeX = sizex;
        sizeY = sizey;
        roomSizeX = 10;
        roomSizeY = 10;
        tiles = new TileType[sizeX*roomSizeX, sizeY * roomSizeY];
        objects = new List<EntityData>();

    }

    public Map(MapData data)
    {
        this.Data = data;
        mapType = data.mapType;
        worldType = data.type;

        sizeX = data.sizeX;
        sizeY = data.sizeY;
        roomSizeX = data.roomSizeX;
        roomSizeY = data.roomSizeY;
        tiles = new TileType[sizeX * roomSizeX, sizeY * roomSizeY];
        gravity = data.gravity;
        objects = new List<EntityData>();

    }

    public TileType[,] GetMap()
    {
        return tiles;
    }

    public Vector2i getMapSize()
    {
        return new Vector2i(sizeX * roomSizeX, sizeY * roomSizeY);
    }

    public TileType GetTile(int x, int y)
    {
        return tiles[x, y];
    }

    public void SetTile(int x, int y, TileType tileType)
    {
        tiles[x, y] = tileType;
    }

    public void SetTileMap(TileType[,] map)
    {
        tiles = map;
    }

    public void AddEntity(EntityData e)
    {
        objects.Add(e);
    }

    
}
