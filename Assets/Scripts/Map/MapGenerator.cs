using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public enum Direction { Left, Up, Right, Down };

public static class MapGenerator {

    static List<TileType[,]> RoomDatabase;

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

    public static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        System.Array.Reverse(charArray);
        return new string(charArray);
    }

    static TileType[,] GetRandomRoom()
    {
        TileType[,] tiles = new TileType[Constants.cMapChunkSizeX, Constants.cMapChunkSizeY];

        TextAsset[] rooms = Resources.LoadAll<TextAsset>("Rooms");

        TextAsset r = rooms[Random.Range(0, rooms.Length)];
        string[] s = r.text.Split(new string[]{System.Environment.NewLine}, System.StringSplitOptions.RemoveEmptyEntries);
        System.Array.Reverse(s);
        for(int k = 0; k < s.Length; k++)
        {
            //s[k] = Reverse(s[k]);
            //s[k] = s[k].Replace(System.Environment.NewLine, "");
            for (int i = 0; i < s[k].Length; i++)
            {
                tiles[k, i] = (TileType)char.GetNumericValue(s[i][k]);
            }
        }

        /*
        //s = s.Replace(System.Environment.NewLine, "");
        s = Reverse(s);
        //Debug.Log(s);
        for (i = 0; i < s.Length; i++)
        {
            row = i / Constants.cMapChunkSizeX;
            column = i % Constants.cMapChunkSizeX;
            
            tiles[column, row] = (TileType)char.GetNumericValue(s[i]);
            
        }
        */
        return tiles;
    }

    static TileType[,] GetRandomRoom2()
    {
        string[] paths =
            Directory.GetFiles(Application.dataPath+"/Rooms", "*.room");

        int r = Random.Range(0, paths.Length);

        TileType[,] temp = new TileType[Constants.cMapChunkSizeX, Constants.cMapChunkSizeY];
        //string path = Path.Combine(Application.persistentDataPath, "test.map");
        using (BinaryReader reader = new BinaryReader(File.OpenRead(paths[r])))
        {
            int header = reader.ReadInt32();
            if (header == 0)
            {
                for (int x = 0; x < Constants.cMapChunkSizeX; x++)
                {
                    for (int y = 0; y < Constants.cMapChunkSizeY; y++)
                    {
                        temp[x, y] = (TileType)reader.ReadByte();
                    }
                }
            }
            else
            {
                Debug.LogWarning("Unknown room format " + header);
            }

        }

        return temp;
    }

    static void LoadRooms()
    {
        //TileType[,] tiles = new TileType[Constants.cMapChunkSizeX, Constants.cMapChunkSizeY];
        RoomDatabase = new List<TileType[,]>();
        string[] paths =
            Directory.GetFiles(Application.persistentDataPath, "*.room");
        

        foreach(string room in paths)
        {
            Debug.Log(room);
            if (!File.Exists(room))
            {
                Debug.LogError("File does not exist " + room);
                return;
            } else
            {
                Debug.Log("File Exists");
            }

            TileType[,] temp = new TileType[Constants.cMapChunkSizeX, Constants.cMapChunkSizeY];
            //string path = Path.Combine(Application.persistentDataPath, "test.map");
            using (BinaryReader reader = new BinaryReader(File.OpenRead(room)))
            {
                int header = reader.ReadInt32();
                if (header == 0)
                {
                    for (int x = 0; x < Constants.cMapChunkSizeX; x++)
                    {
                        for (int y = 0; y < Constants.cMapChunkSizeY; y++)
                        {
                            temp[x, y] = (TileType)reader.ReadByte();
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Unknown room format " + header);
                }

                RoomDatabase.Add(temp);
            }
        }
        
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

    public static Vector2i GetStartTile(MapData map)
    {
        Vector2i startTile = new Vector2i(1,1);
        int xr, yr;
        //int chunkX = Random.Range(0, Constants.cMapChunksX);
        int chunkX = 0;
        int chunkY = 0;

        for (int y = 1; y < Constants.cMapChunkSizeY; y++)
        {
            for (int x = 0; x < Constants.cMapChunkSizeX; x++)
            {
            
                if(map.rooms[chunkX, chunkY].tiles[x,y] == TileType.Empty)
                {
                    if (map.rooms[chunkX, chunkY].tiles[x, y - 1] == TileType.Block)
                    {
                        return startTile = new Vector2i(chunkX * Constants.cMapChunkSizeX + x, chunkY * Constants.cMapChunkSizeY + y);                        
                    }
                }
            }
        }

        return startTile;

    }

    public static void AddDoorTile(MapData map)
    {
        Vector2i doorTile = new Vector2i(99, 99);
        int xr, yr;
        int chunkX = Random.Range(0, Constants.cMapChunksX);
        int chunkY = 3;

        for (int y = 1; y < Constants.cMapChunkSizeY; y++)
        {
            for (int x = 0; x < Constants.cMapChunkSizeX; x++)
            {

                if (map.rooms[chunkX, chunkY].tiles[x, y] == TileType.Empty)
                {
                    if (map.rooms[chunkX, chunkY].tiles[x, y - 1] == TileType.Block)
                    {
                        map.SetTile(chunkX * Constants.cMapChunkSizeX + x, chunkY * Constants.cMapChunkSizeY + y, TileType.Door);
                        //doorTile = new Vector2i(chunkX * Constants.cMapChunkSizeX + x, chunkY * Constants.cMapChunkSizeY + y);
                        return;
                    }
                }
            }
        }

    }

    public static MapData GenerateMap()
    {
        MapData map = new MapData();
        LoadRooms();
        
        for (int x = 0; x < Constants.cMapChunksX; x++)
        {
            for (int y = 0; y < Constants.cMapChunksY; y++)
            {
                map.rooms[x, y].tiles = GetRandomRoom2();
            }
        }

        for (int i = 0; i < Constants.cMapWidth; i++)
        {
            for (int j = 0; j < Constants.cMapHeight; j++)
            {
                
                if(j == 0 || i == 0 || j == Constants.cMapWidth - 1 || i == Constants.cMapHeight - 1)
                {
                    //Debug.Log("X: " + x + ", Y: " + y);
                    map.SetTile(j,i, TileType.Block);
                }
            }
            
        }


        map.startTile = GetStartTile(map);
        AddDoorTile(map);

        return map;
    }


}
