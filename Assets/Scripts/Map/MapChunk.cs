using System.Collections.Generic;
using UnityEngine;

public enum MapChunkType { Corner, Side, Middle };
//[System.Serializable]
public class MapChunk
{
        public TileType[,] tiles;
         
        
        public MapChunk()
        {
            tiles = new TileType[Constants.cMapChunkSizeX, Constants.cMapChunkSizeY];
        }

}

