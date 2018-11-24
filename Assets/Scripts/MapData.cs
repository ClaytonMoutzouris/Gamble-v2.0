﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType { Forest, Tundra };

public class MapData {

    public Vector2i startTile = new Vector2i(1,1);
    public MapChunk[,] rooms;
    public MapType type;

    public MapData(int sizex = Constants.cMapWidth, int sizey = Constants.cMapHeight, MapType type = MapType.Forest)
    {
        this.type = type;
        rooms = new MapChunk[Constants.cMapChunksX, Constants.cMapChunksY];

        for(int x = 0; x < Constants.cMapChunksX; x++)
        {
            for (int y = 0; y < Constants.cMapChunksY; y++)
            {
                rooms[x, y] = new MapChunk(x,y);
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
        if (t == TileType.Block)
            Debug.Log("chunkx: " + chunkX + " chunky: " + chunkY + " xpos: " + xPosInChunk + " ypos: " + yPosInChunk);
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
        TileType[,] tiles = new TileType[Constants.cMapWidth, Constants.cMapWidth];

        for(int x = 0; x < Constants.cMapWidth; x++)
        {
            for (int y = 0; y < Constants.cMapHeight; y++)
            {
                tiles[x, y] = GetTile(x, y);
            }
        }

        return tiles;
    }
}
