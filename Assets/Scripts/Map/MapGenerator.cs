using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public enum Direction { Left, Up, Right, Down };


public static class MapGenerator
{

    static Dictionary<ChunkType, Dictionary<ChunkEdgeType, List<MapChunk>>> ChunkDictionary;




    public static void LoadChunks()
    {
        //Initialize the dictionary
        //This is really horrible, please do it another way!

        ChunkDictionary = new Dictionary<ChunkType, Dictionary<ChunkEdgeType, List<MapChunk>>>();
        for (int chunk = 0; chunk < (int)ChunkType.Count; chunk++)
        {
            ChunkDictionary.Add((ChunkType)chunk, new Dictionary<ChunkEdgeType, List<MapChunk>>());

            for (int edge = 0; edge < (int)ChunkEdgeType.Count; edge++)
            {
                ChunkDictionary[(ChunkType)chunk].Add((ChunkEdgeType)edge, new List<MapChunk>());
            }
        }


        string[] paths =
            Directory.GetFiles(Application.dataPath + "/Rooms/GeneratorRooms/", "*.room", SearchOption.AllDirectories);


        for (int i = 0; i < paths.Length; i++)
        {
            MapChunk temp = new MapChunk();

            //string path = Path.Combine(Application.persistentDataPath, "test.map");
            using (BinaryReader reader = new BinaryReader(File.OpenRead(paths[i])))
            {
                int header = reader.ReadInt32();
                if (header == 1)
                {
                    temp.Load(reader);
                }
                else
                {
                    Debug.LogWarning("Unknown room format " + header);
                }

            }
            ChunkDictionary[temp.type][temp.edgeType].Add(temp.Copy());
            //Debug.Log("Added new with Type: " + temp.type + " and EdgeType: " + temp.edgeType + " To Dictionary");
        }





    }
    /*
    static List<MapChunk> BasicPath(Map map)
    {
        List<MapChunk> rooms = new List<MapChunk>();
        Vector2i currPoint, prevPoint;

        currPoint.x = Random.Range(0, map.MapChunksX);
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

    */

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
                writer.Write(Random.Range(0, 3));
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
        string[] s = r.text.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);
        System.Array.Reverse(s);
        for (int k = 0; k < s.Length; k++)
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

    static MapChunk GetRandomRoom2()
    {
        string[] paths =
            Directory.GetFiles(Application.dataPath + "/Rooms/RandomRooms", "*.room");

        int r = Random.Range(0, paths.Length);

        MapChunk temp = new MapChunk();
        //string path = Path.Combine(Application.persistentDataPath, "test.map");
        using (BinaryReader reader = new BinaryReader(File.OpenRead(paths[r])))
        {
            int header = reader.ReadInt32();
            if (header == 1)
            {
                temp.Load(reader);
            }
            else
            {
                Debug.LogWarning("Unknown room format " + header);
            }

        }

        return temp;
    }



    public static void PrintDictionary()
    {
        foreach (KeyValuePair<ChunkType, Dictionary<ChunkEdgeType, List<MapChunk>>> Types in ChunkDictionary)
        {
            foreach (KeyValuePair<ChunkEdgeType, List<MapChunk>> edges in Types.Value)
            {
                foreach (MapChunk chunk in edges.Value)
                {
                    Debug.Log("Chunk with Type: " + chunk.type + " and EdgeType " + chunk.edgeType);
                }
            }
        }


    }


    static MapChunk GetMapChunk(ChunkType type, ChunkEdgeType edgeType)
    {
        List<MapChunk> chunks = new List<MapChunk>();

        chunks.AddRange(ChunkDictionary[type][edgeType]);
        

        int random = Random.Range(0, chunks.Count);
        //Debug.Log("Chunk with Type: " + chunks[random].type + " and EdgeType " + chunks[random].edgeType);

        return chunks[random].Copy();
    }

    static MapChunk GetHubRoom()
    {

        string path = Directory.GetFiles(Application.dataPath + "/Rooms", "Hub.room")[0];

        //TileType[,] temp = new TileType[Constants.cMapChunkSizeX, Constants.cMapChunkSizeY];
        MapChunk temp = new MapChunk();
        //string path = Path.Combine(Application.persistentDataPath, "test.map");
        using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
        {
            int header = reader.ReadInt32();
            if (header == 1)
            {
                temp.Load(reader);
            }
            else
            {
                Debug.LogWarning("Unknown room format " + header);
            }

        }

        return temp;
    }

    public static Vector2i GetStartTile(Map map, int roomX, int roomY)
    {
        Vector2i startTile = new Vector2i(1, 1);
        //int chunkX = Random.Range(0, Constants.cMapChunksX);

        for (int y = 1; y < Constants.cMapChunkSizeY; y++)
        {
            for (int x = 0; x < Constants.cMapChunkSizeX; x++)
            {

                if (map.rooms[roomX, roomY].tiles[x, y] == TileType.Empty)
                {
                    if (map.rooms[roomX, roomY].tiles[x, y - 1] == TileType.Block)
                    {
                        return startTile = new Vector2i(roomX * Constants.cMapChunkSizeX + x, roomY * Constants.cMapChunkSizeY + y);
                    }
                }
            }
        }

        return startTile;

    }

    public static void AddDoorTile(Map map, int roomX, int roomY)
    {
        Vector2i doorTile = new Vector2i(99, 99);

        for (int y = 1; y < Constants.cMapChunkSizeY; y++)
        {
            for (int x = 0; x < Constants.cMapChunkSizeX; x++)
            {

                if (map.rooms[roomX, roomY].tiles[x, y] == TileType.Empty)
                {
                    if (map.rooms[roomX, roomY].tiles[x, y - 1] == TileType.Block)
                    {
                        map.SetTile(roomX * Constants.cMapChunkSizeX + x, roomY * Constants.cMapChunkSizeY + y, TileType.Door);
                        //doorTile = new Vector2i(chunkX * Constants.cMapChunkSizeX + x, chunkY * Constants.cMapChunkSizeY + y);
                        Debug.Log("Setting Door tile at " + x + "," + y);
                        return;
                    }
                }
            }
        }

    }

    public static void AddBounds(Map map)
    {
        for (int i = 0; i < map.sizeX; i++)
        {
            for (int j = 0; j < map.sizeY; j++)
            {

                if (j == 0 || i == 0 || j == map.sizeY - 1 || i == map.sizeX - 1)
                {
                    Debug.Log("X: " + i + ", Y: " + j);
                    map.SetTile(i, j, TileType.Block);
                }
            }

        }
    }

    public static Map GenerateHubMap()
    {
        Map map = new Map(MapType.Hub, WorldType.Hub, 10, 10);

        for (int x = 0; x < map.MapChunksX; x++)
        {
            for (int y = 0; y < map.MapChunksY; y++)
            {
                map.rooms[x, y] = GetHubRoom();
            }
        }

        //AddBounds(map);

        map.startTile = GetStartTile(map, 0, 0);
        AddDoorTile(map,0,0);


        map.AddEntity(new EnemyData(map.sizeX / 4, map.sizeY / 4, EnemyType.Slime));


        return map;
    }

    public static Map GenerateBossMap(WorldType type)
    {
        Map map = new Map(MapType.BossMap, type, 30, 30);


        for (int i = 0; i < map.sizeX; i++)
        {
            for (int j = 0; j < map.sizeY; j++)
            {

                if (j == 0 || i == 0 || j == map.sizeX - 1 || i == map.sizeY - 1)
                {
                    //Debug.Log("X: " + x + ", Y: " + y);
                    map.SetTile(j, i, TileType.Block);
                }
            }

        }

        map.AddEntity(new EnemyData(map.sizeX / 2, map.sizeY / 2, EnemyType.LavaBoss));

        map.startTile = GetStartTile(map,1,0);
        AddDoorTile(map,0,0);
        return map;
    }


    public static ChunkType[,] GenerateTypeMap(int sizeX, int sizeY, int depth = 5)
    {
        ChunkType[,] typeMap = new ChunkType[sizeX, sizeY];

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                if (y < depth)
                {
                    typeMap[x, y] = ChunkType.Inner;
                }
                else if (y == depth)
                {
                    typeMap[x, y] = ChunkType.Surface;
                }
                else
                {
                    typeMap[x, y] = ChunkType.Above;
                }
            }
        }

        return typeMap;
    }

    public static ChunkEdgeType[,] GenerateEdgeMap(ChunkType[,] typeMap, int sizeX, int sizeY)
    {

        ChunkEdgeType[,] edgeMap = new ChunkEdgeType[sizeX, sizeY];
        
        for(int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                if(typeMap[x,y] == ChunkType.Above)
                {
                    edgeMap[x, y] = ChunkEdgeType.E1111;
                } else if(typeMap[x,y] == ChunkType.Surface)
                {
                    if(x == 0)
                    {
                        edgeMap[x, y] = ChunkEdgeType.E0111;
                    }
                    else if(x == sizeX-1)
                    {
                        edgeMap[x, y] = ChunkEdgeType.E1101;
                    }
                    else
                    {
                        edgeMap[x, y] = ChunkEdgeType.E1011;
                    }
                } else if(typeMap[x, y] == ChunkType.Inner)
                {
                    //Left wall first
                    if(x == 0)
                    {
                        //Bottom left corner
                        if(y == 0)
                        {
                            edgeMap[x, y] = ChunkEdgeType.E0011;
                        } else
                        {
                            edgeMap[x, y] = ChunkEdgeType.E0111;
                        }

                    } else if(x == sizeX - 1) //right wall next
                    {
                        //Bottom right corner
                        if (y == 0)
                        {
                            edgeMap[x, y] = ChunkEdgeType.E1001;
                        }
                        else
                        {
                            edgeMap[x, y] = ChunkEdgeType.E1101;
                        }
                    } else
                    {
                        //Bottom middle
                        if (y == 0)
                        {
                            edgeMap[x, y] = ChunkEdgeType.E1011;
                        }
                        else
                        {
                            edgeMap[x, y] = ChunkEdgeType.E1111;
                        }
                    }
                }

            }
        }
        //edgeMap[startingX, startingY] = ChunkEdgeType.E1111;
        



        return edgeMap;
    }


    public static List<Direction> PossibleDirections(ChunkNode node, int maxX, int maxY)
    {
        List<Direction> directions = new List<Direction>();

        if(node.x != 0)
        {
            directions.Add(Direction.Left);
        }
        if (node.y != 0)
        {
            directions.Add(Direction.Down);
        }
        if (node.x != maxX-1)
        {
            directions.Add(Direction.Right);
        }
        if (node.y != maxY-1)
        {
            directions.Add(Direction.Up);
        }

        return directions;

    }

    public static ChunkNode[,] GenerateNodeMap(int sizeX, int sizeY, int startingX, int startingY, int goalX, int goalY, int depth = 5)
    {
        ChunkNode[,] nodeMap = new ChunkNode[sizeX, sizeY];

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                //Check if chunk lies on an edge.
                //Check if tile is 
                nodeMap[x, y] = new ChunkNode(x, y);

                if(x != 0)
                {
                    nodeMap[x, y].AddNeighbour(Direction.Left,nodeMap[x-1,y]);
                    nodeMap[x-1, y].AddNeighbour(Direction.Right, nodeMap[x, y]);
                }
                if(y != 0)
                {
                    nodeMap[x, y].AddNeighbour(Direction.Down, nodeMap[x, y-1]);
                    nodeMap[x, y-1].AddNeighbour(Direction.Up, nodeMap[x, y]);
                }
                if(x != sizeX - 1)
                {
                    nodeMap[x, y].AddNeighbour(Direction.Right, nodeMap[x + 1, y]);
                }
                if(y != sizeY - 1)
                {
                    nodeMap[x, y].AddNeighbour(Direction.Up, nodeMap[x, y + 1]);
                }
            }
        }



        ChunkNode currentNode = nodeMap[startingX, startingY];
        bool pathDone = false;
        Debug.Log("Path X: " + currentNode.x + ", Y: " + currentNode.y);


        while (!pathDone)
        {
            List<Direction> dirs = PossibleDirections(currentNode, sizeX, sizeY);

            if(dirs.Count > 0)
            {
                Direction next = dirs[Random.Range(0, dirs.Count)];
                Debug.Log("Next Direction is " + next.ToString());

                currentNode.ChangeEdge(next, true);
                currentNode = currentNode.GetNeighbour(next);
                Debug.Log("Path X: " + currentNode.x + ", Y: " + currentNode.y);
                if(currentNode == nodeMap[goalX, goalY])
                {
                    pathDone = true;
                }
            }
            else
            {
                //we're fucked
                pathDone = true;
            }

        }

        return nodeMap;
    }

    public static Map GenerateMap()
    {
        Map map = new Map(MapType.World, (WorldType)Random.Range(0, (int)WorldType.Count));
        //LoadRooms();
        int depth = Random.Range(6, 8);
        //PrintDictionary();
        int startingX = Random.Range(0, map.MapChunksX);
        int startingY = depth;

        int goalX = Random.Range(0, map.MapChunksX);
        int goalY = Random.Range(0, 2);
        ChunkNode[,] nodemap = GenerateNodeMap(map.MapChunksX, map.MapChunksY, startingX, startingY, goalX, goalY);

        ChunkType[,] typeMap = GenerateTypeMap(map.MapChunksX, map.MapChunksY, depth);
        ChunkEdgeType[,] edgeMap = GenerateEdgeMap(typeMap, map.MapChunksX, map.MapChunksY);

        for (int x = 0; x < map.MapChunksX; x++)
        {
            for (int y = 0; y < map.MapChunksY; y++)
            {
                map.rooms[x, y] = GetMapChunk(typeMap[x,y], edgeMap[x,y]);
            }
        }

        AddBounds(map);


        map.startTile = GetStartTile(map, startingX, startingY);
        AddDoorTile(map, goalX, goalY);

        PopulateMap(map);

        return map;
    }

    static void PopulateMap(Map map)
    {
        AddChests(map);
        AddEnemies(map);
        AddFallingRock(map);



    }

    static void AddChests(Map map)
    {
        int r;

        for (int x = 0; x < map.sizeX; x++)
        {
            for (int y = 0; y < map.sizeY; y++)
            {
                r = Random.Range(0, 100);
                if (r < 95)
                    continue;

                if (map.GetTile(x, y) == TileType.Empty)
                {
                    if (y != 0 && map.GetTile(x, y - 1) == TileType.Block)
                        map.AddEntity(new ObjectData(x, y, ObjectType.Chest));
                }
            }
        }
    }

    static void AddFallingRock(Map map)
    {
        int r;

        for (int x = 0; x < map.sizeX; x++)
        {
            for (int y = 0; y < map.sizeY; y++)
            {
                r = Random.Range(0, 100);
                if (r < 95)
                    continue;

                if (map.GetTile(x, y) == TileType.Empty)
                {
                    if (y != (map.sizeY - 1) && map.GetTile(x, y + 1) == TileType.Block)
                        map.AddEntity(new ObjectData(x, y, ObjectType.FallingRock));
                }
            }
        }
    }

    static void AddEnemies(Map map)
    {
        int r;

        for (int x = 0; x < map.sizeX; x++)
        {
            for (int y = 0; y < map.sizeY; y++)
            {
                r = Random.Range(0, 100);
                if (r < 98)
                    continue;

                if (map.GetTile(x, y) == TileType.Empty)
                {
                    if (y != 0 && map.GetTile(x, y - 1) == TileType.Block)
                    {
                        int enemyRandom = Random.Range(0, (int)EnemyType.Count);
                        map.AddEntity(new EnemyData(x, y, (EnemyType)enemyRandom));
                    }
                }
            }
        }
    }




}
