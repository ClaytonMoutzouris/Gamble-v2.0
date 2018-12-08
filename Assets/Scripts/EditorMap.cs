using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EditorMap : Map {



    public override void Init()
    {
        mWidth = Constants.cMapChunkSizeX;
        mHeight = Constants.cMapChunkSizeY;

        //set the position
        mPosition = transform.position;
        Debug.Log("Here");
        mTileData = new TileType[mWidth, mHeight];

        for(int x = 0; x < mWidth; x++)
        {
            for (int y = 0; y < mHeight; y++)
            {
                if(x == 0 || x == mWidth-1 || y == 0 || y == mHeight - 1) 
                mTileData[x, y] = TileType.Block;
            }
        }

        mTileMap.DrawMap(mTileData, mWidth, mHeight);

    }
}
