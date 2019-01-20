using System.Collections.Generic;
using UnityEngine;
using System.IO;

//NESW

public enum ChunkType { Above, Surface, Inner, Count };
public enum ChunkEdge { North, East, West, South }
//Each bit represents a direction W S E N -> 0 = closed edge, 1 = open edge
public enum ChunkEdgeType { E0000, E0001, E0010, E0011, E0100, E0101, E0110, E0111, E1000, E1001, E1010, E1011, E1100, E1101, E1110, E1111, Count };

//[System.Serializable]
public class MapChunk
{
    public TileType[,] tiles;
    //public Dictionary<ChunkEdge, bool> chunkEdges; //This dictionary keeps track of which sides are open and which are closed?
    public ChunkType type;
    public ChunkEdgeType edgeType;
    public int mWidth;
    public int mHeight;
        
    public MapChunk()
    {
        mWidth = Constants.cMapChunkSizeX;
        mHeight = Constants.cMapChunkSizeY;


        tiles = new TileType[mWidth, mHeight];
    }

    public MapChunk Copy()
    {
        MapChunk copy = new MapChunk();
        copy.tiles = tiles.Clone() as TileType[,];
        copy.type = this.type;
        copy.edgeType = this.edgeType;
        copy.mWidth = this.mWidth;
        copy.mHeight = this.mHeight;

        return copy;
    }


    public void Load(BinaryReader reader)
    {
        type = (ChunkType)reader.ReadByte();

        edgeType = (ChunkEdgeType)reader.ReadByte();


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

        //Then write the EdgeType
        writer.Write((byte)edgeType);


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

