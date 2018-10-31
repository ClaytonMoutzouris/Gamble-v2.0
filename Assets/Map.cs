using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    [SerializeField]
    public TileType[,] mTiles;

    public TileMapObject mTileMap;

    public Vector3 mPosition;

    public int mWidth = 80;
    public int mHeight = 60;

    public const int cTileSize = 32;

    public void Start()
    {
        InitDefaultMap();
        mTileMap.DrawMap(mTiles, mWidth, mHeight);
    }

    void InitDefaultMap()
    {
        mTiles = new TileType[mWidth, mHeight];

        for (int x = 0; x < mWidth; x++)
        {
            for (int y = 0; y < mHeight; y++)
            {
                mTiles[x, y] = TileType.Empty;
            }
        }

        for (int x = 0; x < mWidth; x++)
        {
                mTiles[x, 0] = TileType.Block;
        }

        mTiles[0, 2] = TileType.OneWay;
        mTiles[1, 2] = TileType.OneWay;
        mTiles[2, 2] = TileType.OneWay;

    }

    public Vector2i GetMapTileAtPoint(Vector2 point)
    {
        return new Vector2i((int)((point.x - mPosition.x + cTileSize / 2.0f) / (float)(cTileSize)),
                    (int)((point.y - mPosition.y + cTileSize / 2.0f) / (float)(cTileSize)));
    }

    public int GetMapTileYAtPoint(float y)
    {
        return (int)((y - mPosition.y + cTileSize / 2.0f) / (float)(cTileSize));
    }

    public int GetMapTileXAtPoint(float x)
    {
        return (int)((x - mPosition.x + cTileSize / 2.0f) / (float)(cTileSize));
    }

    public Vector2 GetMapTilePosition(int tileIndexX, int tileIndexY)
    {
        return new Vector2(
                (float)(tileIndexX * cTileSize) + mPosition.x,
                (float)(tileIndexY * cTileSize) + mPosition.y
            );
    }

    public Vector2 GetMapTilePosition(Vector2i tileCoords)
    {
        return new Vector2(
            (float)(tileCoords.x * cTileSize) + mPosition.x,
            (float)(tileCoords.y * cTileSize) + mPosition.y
            );
    }

    public TileType GetTile(int x, int y)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return TileType.Block;

        return mTiles[x, y];
    }

    public bool IsObstacle(int x, int y)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return true;

        return (mTiles[x, y] == TileType.Block);
    }

    public bool IsGround(int x, int y)
    {
        if (x < 0 || x >= mWidth
           || y < 0 || y >= mHeight)
            return false;

        return (mTiles[x, y] == TileType.OneWay || mTiles[x, y] == TileType.Block);
    }

    public bool IsOneWayPlatform(int x, int y)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return false;

        return (mTiles[x, y] == TileType.OneWay);
    }

    public bool IsEmpty(int x, int y)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return false;

        return (mTiles[x, y] == TileType.Empty);
    }

}
