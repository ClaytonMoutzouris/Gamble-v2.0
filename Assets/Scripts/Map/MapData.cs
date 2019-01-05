using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapData {

    public Vector2i startTile = new Vector2i(1,1);
    public MapChunk[,] rooms;
    public WorldType type;
    public MapType mapType;
    public int sizeX;
    public int sizeY;

    public int MapChunksX
    {
        get
        {
            return sizeX / Constants.cMapChunkSizeX;
        }
    }

    public int MapChunksY
    {
        get
        {
            return sizeY / Constants.cMapChunkSizeY;
        }
    }



    public List<EntityData> objects;

    public MapData(MapType mapType = MapType.Hub, WorldType type = WorldType.Forest, int sizex = Constants.cDefaultMapWidth, int sizey = Constants.cDefaultMapHeight)
    {
        this.mapType = mapType;
        this.type = type;
        sizeX = sizex;
        sizeY = sizey;

        rooms = new MapChunk[MapChunksX, MapChunksY];
        objects = new List<EntityData>();

        for (int x = 0; x < MapChunksX; x++)
        {
            for (int y = 0; y < MapChunksY; y++)
            {
                rooms[x, y] = new MapChunk();
            }
        }
    }

    public void SetTile(int x, int y, TileType t)
    {
        int chunkX, chunkY, xPosInChunk, yPosInChunk;
        chunkX = x / Constants.cMapChunkSizeX;
        chunkY = y / Constants.cMapChunkSizeY;
        xPosInChunk = x % Constants.cMapChunkSizeX;
        yPosInChunk = y % Constants.cMapChunkSizeY;
        //if (t == TileType.Block)
            //Debug.Log("chunkx: " + chunkX + " chunky: " + chunkY + " xpos: " + xPosInChunk + " ypos: " + yPosInChunk);
        rooms[chunkX, chunkY].tiles[xPosInChunk, yPosInChunk] = t;
    }
    

    public TileType GetTile(int x, int y)
    {
        int chunkX, chunkY, xPosInChunk, yPosInChunk;
        chunkX = x / Constants.cMapChunkSizeX;
        chunkY = y / Constants.cMapChunkSizeY;
        xPosInChunk = x % Constants.cMapChunkSizeX;
        yPosInChunk = y % Constants.cMapChunkSizeY;
        return rooms[chunkX, chunkY].tiles[xPosInChunk, yPosInChunk];
    }

    public TileType[,] GetMap()
    {
        TileType[,] tiles = new TileType[sizeX, sizeY];

        for(int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                tiles[x, y] = GetTile(x, y);
            }
        }

        return tiles;
    }

    public void AddEntity(EntityData e)
    {
        objects.Add(e);
    }
}
