using System.Collections.Generic;
using UnityEngine;
using System.IO;

//NESW

public enum ChunkType { Above, Surface, Inner };
public enum ChunkOrientation { }
//[System.Serializable]
public class MapChunk
{
    public TileType[,] tiles;
    public ChunkType type;
    public int mWidth;
    public int mHeight;

    public MapChunk()
    {
        mWidth = Constants.cMapChunkSizeX;
        mHeight = Constants.cMapChunkSizeY;


        tiles = new TileType[mWidth, mHeight];
    }


    public void Load(BinaryReader reader)
    {
        type = (ChunkType)reader.ReadByte();
        mWidth = reader.ReadByte();
        mHeight = reader.ReadByte();

        for (int x = 0; x < mWidth; x++)
        {
            for (int y = 0; y < mHeight; y++)
            {

                tiles[x, y] = (TileType)reader.ReadByte();
            }
        }
    }

    public void Save(BinaryWriter writer)
    {
        //First write the type
        writer.Write((byte)type);

        //Then write the size
        writer.Write((byte)mWidth);
        writer.Write((byte)mHeight);


        for (int x = 0; x < mWidth; x++)
        {
            for (int y = 0; y < mHeight; y++)
            {
                //For maximum efficiency, we store the tiletype as a byte,
                //since its an int within the 0-255 range for now
                //writer.Write((byte)mMapData.type);
                writer.Write((byte)tiles[x, y]);
            }
        }

    }
}

