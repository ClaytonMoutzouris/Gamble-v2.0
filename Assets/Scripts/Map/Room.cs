using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class Room
{
    public TileType[,] tiles;
    public RoomType roomType;
    public SurfaceLayer layer = SurfaceLayer.Surface;
    public int mWidth;
    public int mHeight;

    public Room(){
        mWidth = Constants.cMapChunkSizeX;
        mHeight = Constants.cMapChunkSizeY;
        roomType = RoomType.SideRoom;
        tiles = new TileType[mWidth, mHeight];
    }

    public Room(RoomType type)
    {
        mWidth = Constants.cMapChunkSizeX;
        mHeight = Constants.cMapChunkSizeY;
        //layer = layerType;
        roomType = type;
        tiles = new TileType[mWidth, mHeight];
    }

   

    public Room Copy()
    {
        Room copy = new Room();
        copy.tiles = tiles.Clone() as TileType[,];
        copy.roomType = this.roomType;
        copy.mWidth = this.mWidth;
        copy.mHeight = this.mHeight;

        return copy;
    }


    public void Load(BinaryReader reader)
    {
        roomType = (RoomType)reader.ReadByte();


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
        writer.Write((byte)roomType);

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
