using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EditorMap : MapManager
{

    public MapChunk chunk;

    public override void Init()
    {
        mWidth = Constants.cMapChunkSizeX;
        mHeight = Constants.cMapChunkSizeY;
        //set the position
        mPosition = transform.position;

        chunk = new MapChunk();
        mTileData = chunk.tiles;
        //chunk.type = ChunkType.Inner;

        for (int x = 0; x < mWidth; x++)
        {
            for (int y = 0; y < mHeight; y++)
            {
                if (x == 0 || x == mWidth - 1 || y == 0 || y == mHeight - 1)
                    mTileData[x, y] = TileType.Block;
            }
        }

        mTileMap.DrawMap(mTileData, mWidth, mHeight);

    }

    public void Draw()
    {
        mTileMap.DrawMap(mTileData, mWidth, mHeight);
    }

}
