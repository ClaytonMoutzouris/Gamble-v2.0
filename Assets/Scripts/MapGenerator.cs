using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public enum Direction { Left, Up, Right, Down };

public static class MapGenerator {

    

    static List<MapChunk> BasicPath()
    {
        List<MapChunk> rooms = new List<MapChunk>();
        Vector2i currPoint, prevPoint;

        currPoint.x = Random.Range(0, Constants.cMapChunksX);
        currPoint.y = 0; //Start in a bottom room
        prevPoint = currPoint;
        //The path can move left or right or up
        //The path cannot move left if it just moved right, unless it moved up, and vice versa
        //The path cannot move left or right if it would move out of bounds of the map

        //Add our starting room
       // rooms.Add(new MapChunk(currPoint.x, currPoint.y));

        //lets create an array to track if the way we want to move is legal
        //It will move closewise starting with left, so left - [0], up - [1], right - [2]
        List<Direction> legalDirs;
        int r = 0;
        while (currPoint.y < Constants.cMapChunksY-1)
        {
            legalDirs = GetLegalDirections(currPoint, prevPoint);
            r = Random.Range(0, legalDirs.Count);
            prevPoint = currPoint;

            switch (legalDirs[r])
            {
                case Direction.Left:
                    currPoint.x--;
                    //rooms.Add(new MapChunk(currPoint.x, currPoint.y));
                    break;
                case Direction.Up:
                    currPoint.y++;
                    //rooms.Add(new MapChunk(currPoint.x, currPoint.y));
                    break;
                case Direction.Right:
                    currPoint.x++;
                    //rooms.Add(new MapChunk(currPoint.x, currPoint.y));
                    break;
            }
            
        }

        

        return rooms;
    }

    
    [MenuItem("Assets/Create/EmptyRoom")]
    static void WriteString()
    {
        
        int index = Resources.LoadAll<TextAsset>("Rooms").Length;
        Resources.UnloadUnusedAssets();

        string path = "Assets/Resources/Rooms/EmptyRoom" + index + ".txt";
        

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        for (int y = 0; y < Constants.cMapChunkSizeY; y++)
        {
            for (int x = 0; x < Constants.cMapChunkSizeX; x++)
            {
                writer.Write(Random.Range(0,3));
            }
            writer.Write(System.Environment.NewLine);
        }

        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
    }

    static TileType[,] GetRandomRoom()
    {
        TextAsset[] rooms = Resources.LoadAll<TextAsset>("Rooms");

        TextAsset r = rooms[Random.Range(0, rooms.Length)];
        string s = r.text;
        int i, j, column, row;
        TileType[,] tiles = new TileType[Constants.cMapChunkSizeX, Constants.cMapChunkSizeY];
        s = s.Replace(System.Environment.NewLine, "");
        //Debug.Log(s);
        for (i = 0; i < s.Length; i++)
        {
            column = i % Constants.cMapChunkSizeX;
            row = i/Constants.cMapChunkSizeX;
            tiles[column, row] = (TileType)char.GetNumericValue(s[i]);
            
        }

        return tiles;
    }

    //This is for creating the initial path of rooms
    static List<Direction> GetLegalDirections(Vector2i currPoint, Vector2i prevPoint)
    {
        List<Direction> legalDirections = new List<Direction>();


        if(currPoint.x != 0 && (currPoint.x < prevPoint.x || currPoint.y != prevPoint.y))
        {
            legalDirections.Add(Direction.Left);
        }

        legalDirections.Add(Direction.Up);


        if (currPoint.x != Constants.cMapChunksX-1 && (currPoint.x > prevPoint.x || currPoint.y != prevPoint.y))
        {
            legalDirections.Add(Direction.Right);
        }

        return legalDirections;
    }

    public static TileType[,] GenerateMap()
    {
        MapData map = new MapData();


        for (int x = 0; x < Constants.cMapChunksX; x++)
        {
            for (int y = 0; y < Constants.cMapChunksY; y++)
            {
                map.rooms[x, y].tiles = GetRandomRoom();
            }
        }



                /*
                Debug.Log("The chunk of tile 3,4 is: " + ((3 / Constants.cMapChunkSizeX) + (4 / Constants.cMapChunkSizeY) * (sizex / Constants.cMapChunkSizeX)));
                Debug.Log("The chunk of tile 78,28 is: " + ((78 / Constants.cMapChunkSizeX) + (28 / Constants.cMapChunkSizeY) * (sizex / Constants.cMapChunkSizeX)));
                int chunkX = 99 / Constants.cMapChunkSizeX;
                int chunkY = 99 / Constants.cMapChunkSizeY;

                Debug.Log("The chunk of tile 100, 100 is: " + (chunkX + chunkY * chunkCountX));
                */
                //int prevChunk = 0;
                //int currentChunk = 0;

                //Debug.Log("Chunk: " + currentChunk);





        for (int x = 0; x < Constants.cMapWidth; x++)
        {
            for (int y = 0; y < Constants.cMapWidth; y++)
            {
                if(x == 0 || y == 0 || x == Constants.cMapWidth - 1 || y == Constants.cMapWidth - 1)
                {
                    map.SetTile(x,y,TileType.Block);
                } 

            }
        }
        

        return map.GetMap();
    }


}
