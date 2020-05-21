using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum RoomType { Hub, SideRoom, LeftRight, LeftRightBottom, LeftRightBottomTop, BossRoom, MinibossRoom, Count };
public enum RoomType2 { Hub, UpDownLeftRight, UpDownLeft, UpDownRight, DownLeftRight,  UpLeftRight, UpDown, UpLeft, UpRight, DownLeft, DownRight, LeftRight, Up, Down, Left, Right }

public class RoomData
{
    public TileType[,] tiles;
    public RoomType roomType;
    public WorldType worldType;
    public SurfaceLayer surfaceLayer;
    public int mWidth;
    public int mHeight;

    public RoomData(RoomType type = RoomType.SideRoom, int sizex = Constants.cRoomSizeX, int sizey = Constants.cRoomSizeY)
    {
        mWidth = sizex;
        mHeight = sizey;
        roomType = type;
        tiles = new TileType[mWidth, mHeight];
    }

    public RoomData Copy()
    {
        RoomData copy = new RoomData();
        copy.tiles = tiles.Clone() as TileType[,];
        copy.roomType = roomType;
        copy.worldType = worldType;
        copy.surfaceLayer = surfaceLayer;
        copy.mWidth = mWidth;
        copy.mHeight = mHeight;

        return copy;
    }


    public void Load(BinaryReader reader)
    {
        roomType = (RoomType)reader.ReadByte();
        worldType = (WorldType)reader.ReadByte();
        surfaceLayer = (SurfaceLayer)reader.ReadByte();
        //Debug.Log("Reading Surface Type " + surfaceLayer);

        mWidth = reader.ReadByte();
        mHeight = reader.ReadByte();

        tiles = new TileType[mWidth, mHeight];

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
        writer.Write((byte)worldType);
        writer.Write((byte)surfaceLayer);

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