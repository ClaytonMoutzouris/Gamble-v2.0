using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using AccidentalNoise;


public enum Direction { Left, Up, Right, Down };


public static class MapGenerator
{

    static Dictionary<RoomType, List<Room>> RoomDictionary;


    public static void LoadRooms()
    {
        RoomDictionary = new Dictionary<RoomType, List<Room>>();

        for (int type = 0; type < (int)RoomType.Count; type++)
        {
            RoomDictionary.Add((RoomType)type, new List<Room>());
        }

        string[] paths = Directory.GetFiles(Application.dataPath + "/Rooms", "*.room", SearchOption.AllDirectories);


        for (int i = 0; i < paths.Length; i++)
        {
            Room temp = new Room();

            //string path = Path.Combine(Application.persistentDataPath, "test.map");
            using (BinaryReader reader = new BinaryReader(File.OpenRead(paths[i])))
            {
                int header = reader.ReadInt32();
                Debug.Log("Reading room with header " + header);
                if (header == 1)
                {
                    temp.Load(reader);
                }
                else
                {
                    Debug.LogWarning("Unknown room format " + header);
                }

            }
            RoomDictionary[temp.roomType].Add(temp.Copy());
        }
    }

    static Room GetRoom(RoomType type)
    {
        List<Room> chunks = new List<Room>();
        try
        {
            chunks.AddRange(RoomDictionary[type]);
        }
        catch (System.NullReferenceException)
        {
            Debug.Log(type + " " + chunks.Count);
        }

        int random = Random.Range(0, chunks.Count);
        //Debug.Log("Chunk with Type: " + chunks[random].type + " and EdgeType " + chunks[random].edgeType);

        return chunks[random].Copy();
    }

    static Room GetHubRoom()
    {

        string path = Directory.GetFiles(Application.dataPath + "/Rooms", "Hub.room")[0];

        //TileType[,] temp = new TileType[Constants.cMapChunkSizeX, Constants.cMapChunkSizeY];
        Room temp = new Room();
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
        for (int i = 0; i < map.sizeY; i++)
        {
            for (int j = 0; j < map.sizeX; j++)
            {

                if (j == 0 || i == 0 || j == map.sizeX - 1 || i == map.sizeY - 1)
                {
                    //Debug.Log("X: " + i + ", Y: " + j);
                    map.SetTile(j, i, TileType.Block);
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
                map.rooms[x, y] = GetRoom(RoomType.Hub);
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

    //For Generating a map based on noise
    public static Map GenerateMap2()
    {
        Map map = new Map(MapType.World, (WorldType)Random.Range(0, (int)WorldType.Count));
        int width = map.sizeX;
        int height = map.sizeY;
        int depth = Random.Range(6, 8);
        //PrintDictionary();
        int startingX = Random.Range(0, map.MapChunksX);
        int startingY = depth;

        int goalX = Random.Range(0, map.MapChunksX);
        int goalY = Random.Range(0, 2);
        uint seed = (uint)Random.Range(0, 10000);
        ModuleBase combinedTerrain = TerrainPresets.Caves();
        SMappingRanges ranges = new SMappingRanges();
        float scale = 1f;

        for (int x = width-1; x >= 0; x--)
        {
            for (int y = height - 1; y >= 0; y--)
            {
                double p = (double)x / (double)width;
                double q = (double)y / (double)height;
                double nx, ny = 0.0;
                nx = ranges.mapx0 + p * (ranges.mapx1 - ranges.mapx0);
                ny = ranges.mapy0 + q * (ranges.mapy1 - ranges.mapy0);

                float val = (float)combinedTerrain.Get(nx * scale, ny * scale);
                if (val > 0.99)
                {
                    map.SetTile(width - 1-x, height - 1- y, TileType.Block);
                } else
                {
                    map.SetTile(width - 1 - x, height - 1 - y, TileType.Empty);

                }

                //texture.SetPixel(x, y, new Color(val, val, val));
            }
        }

        AddBounds(map);

        Vector2i start = GetStartTile(map, startingX, 4);
        map.startTile = start;
        Debug.Log("Start tile " + start);
        AddDoorTile(map, goalX, goalY);

        PopulateMap(map);

        return map;
    }

    public static Room[,] RoomMap(int sizeX, int sizeY, out Vector2i startRoom, out Vector2i endRoom, int depth = 5)
    {
        Room[,] map = new Room[sizeX, sizeY];
        List<Room> mainPath = new List<Room>();
        startRoom = new Vector2i(Random.Range(0, sizeX), depth);

        map[startRoom.x, startRoom.y] = new Room(RoomType.LeftRightBottomTop);
        int currentX = startRoom.x;
        int currentY = startRoom.y;
        int prevX = currentX;
        int prevY = currentY;

        //Debug.Log("StartingX: " + startingX + ", StartingY: " + startingY);

        mainPath.Add(map[currentX, currentY]);
        int r = 0;
        bool done = false;

        while (!done)
        {
            r = Random.Range(0, 5);
            prevX = currentX;
            prevY = currentY;

            if(r < 2) //Move left
            {
                if(currentX != 0)
                {
                    currentX -= 1;
                    map[currentX, currentY] = new Room(RoomType.LeftRight);
                } else
                {
                    if(map[currentX, currentY].roomType == RoomType.LeftRight)
                    map[currentX, currentY].roomType = RoomType.LeftRightBottomTop;

                    if (currentY != 0)
                    {
                        currentX += 1;
                        map[currentX, currentY] = new Room(RoomType.LeftRightBottomTop);

                        currentY -= 1;
                        map[currentX, currentY] = new Room(RoomType.LeftRight);
                    }
                    else
                    {
                        done = true;

                    }
                }
            } else if(r < 4) //Move Right
            {
                if (currentX != sizeX-1)
                {
                    currentX += 1;
                    map[currentX, currentY] = new Room(RoomType.LeftRight);
                }
                else
                {
                    if (map[currentX, currentY].roomType == RoomType.LeftRight)
                        map[currentX, currentY].roomType = RoomType.LeftRightBottomTop;

                    if (currentY != 0)
                    {
                        currentX -= 1;
                        map[currentX, currentY] = new Room(RoomType.LeftRightBottomTop);

                        currentY -= 1;
                        map[currentX, currentY] = new Room(RoomType.LeftRight);
                    }
                    else
                    {
                        done = true;

                    }
                }
            } else //Move Down
            {
                if (currentY != 0)
                {
                    currentY -= 1;
                    map[currentX, currentY] = new Room(RoomType.LeftRight);
                }
                else
                {
                    done = true;

                }
            }

            if(map[prevX, prevY].roomType == RoomType.LeftRightBottomTop)
            {
                map[currentX, currentY].roomType = RoomType.LeftRightBottomTop;
            }
            mainPath.Add(map[currentX, currentY]);
        }
        endRoom = new Vector2i(currentX, currentY);

        return map;
    }

    public static Map GenerateMap()
    {
        Map map = new Map(MapType.World, (WorldType)Random.Range(0, (int)WorldType.Count));

        int depth = Random.Range(8, 9);
        Vector2i startRoom;
        Vector2i endRoom;


        Room[,] roomPath = RoomMap(map.MapChunksX, map.MapChunksY, out startRoom, out endRoom, depth);

        for (int x = 0; x < map.MapChunksX; x++)
        {
            for (int y = 0; y < map.MapChunksY; y++)
            {
                if(roomPath[x, y] != null)
                    map.rooms[x, y] = GetRoom(roomPath[x,y].roomType);
                else
                    map.rooms[x, y] = GetRoom(RoomType.SideRoom);

            }
        }

        AddBounds(map);


        map.startTile = GetStartTile(map, startRoom.x, startRoom.y);
        AddDoorTile(map, endRoom.x, endRoom.y);

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
