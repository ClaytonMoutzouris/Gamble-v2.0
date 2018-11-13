using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChunk
{

        public int Left;
        public int Right;
        public int Top;
        public int Bottom;
        public TileType[,] tiles;

        public MapChunk(int xIndex, int yIndex)
        {
            Left = xIndex * Constants.cMapChunkSizeX;
            Right = xIndex * Constants.cMapChunkSizeX + Constants.cMapChunkSizeX - 1;
            Top = yIndex * Constants.cMapChunkSizeY + Constants.cMapChunkSizeY - 1;
            Bottom = yIndex * Constants.cMapChunkSizeY;

            tiles = new TileType[Constants.cMapChunkSizeX, Constants.cMapChunkSizeY];


        }

        public override string ToString()
        {
            return "Left: " + Left + " Right: " + Right + " Top: " + Top + " Bottom: " + Bottom;
        }


}

