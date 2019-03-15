using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using AccidentalNoise;


public enum Direction { Invalid, Up, Left, Down, Right };


public static class MapGenerator
{

    static Dictionary<RoomType, List<Room>> RoomDictionary;

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

    public static Vector2i GetStartTile(Map map, int roomX, int roomY)
    {
        Vector2i startTile = new Vector2i(1, 1);
        List<Vector2i> possibleTiles = new List<Vector2i>();

        //int chunkX = Random.Range(0, Constants.cMapChunksX);

        for (int y = 1; y < Constants.cMapChunkSizeY; y++)
        {
            for (int x = 0; x < Constants.cMapChunkSizeX; x++)
            {

                if (map.rooms[roomX, roomY].tiles[x, y] == TileType.Empty)
                {
                    if (map.rooms[roomX, roomY].tiles[x, y - 1] == TileType.Block)
                    {
                        possibleTiles.Add(new Vector2i(roomX * Constants.cMapChunkSizeX + x, roomY * Constants.cMapChunkSizeY + y));
                    }
                }
            }
        }

        startTile = possibleTiles[Random.Range(0, possibleTiles.Count)];


        return startTile;

    }

    public static Vector2i AddDoorTile(Map map, int roomX, int roomY)
    {
        List<Vector2i> possibleTiles = new List<Vector2i>();
        Vector2i tile = new Vector2i(1, 1);

        for (int y = 1; y < Constants.cMapChunkSizeY; y++)
        {
            for (int x = 0; x < Constants.cMapChunkSizeX; x++)
            {

                if (map.rooms[roomX, roomY].tiles[x, y] == TileType.Empty)
                {
                    if (map.rooms[roomX, roomY].tiles[x, y - 1] == TileType.Block)
                    {
                        possibleTiles.Add(new Vector2i(roomX * Constants.cMapChunkSizeX + x, roomY * Constants.cMapChunkSizeY + y));
                    }
                }
            }
        }

        tile = possibleTiles[Random.Range(0, possibleTiles.Count)];

        return tile;

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
        Vector2i door = AddDoorTile(map, goalX, goalY);
        Debug.Log("Door tile " + door);

        map.SetTile(door.x, door.y, TileType.Door);

        PopulateMap(map);

        return map;
    }

    /*
    public static Node<Room>[,] NodeMap(int sizeX, int sizeY, out Vector2i startRoom, out Vector2i endRoom, int depth)
    {
        startRoom = new Vector2i(Random.Range(0, sizeX), depth);
        endRoom = new Vector2i(0, 0);
        Node<Room>[,] map = new Node<Room>[sizeX, sizeY];


        

    }
    */
   

    public static RoomData[,] RoomMap(int sizeX, int sizeY, Vector2i startRoom, out Vector2i endRoom, int depth)
    {
        RoomData[,] map = new RoomData[sizeX, sizeY];
        List<RoomData> mainPath = new List<RoomData>();

        map[startRoom.x, startRoom.y] = new RoomData(RoomType.LeftRightBottomTop);
        int currentX = startRoom.x;
        int currentY = startRoom.y;
        int prevX = currentX;
        int prevY = currentY;


        mainPath.Add(map[currentX, currentY]);
        int r = 0;
        bool done = false;
        Direction lastDirection = Direction.Invalid;

        while (!done)
        {
            r = Random.Range(0, 5);

            if (r < 2) //Move left
            {
                if(currentX != 0 && lastDirection != Direction.Right)
                {
                    lastDirection = Direction.Left;
                    prevX = currentX;
                    currentX -= 1;
                    map[currentX, currentY] = new RoomData(RoomType.LeftRight);
                } else
                {
                    //if(map[currentX, currentY].roomType == RoomType.LeftRight)
                    map[currentX, currentY].roomType = RoomType.LeftRightBottomTop;

                    if (currentY != 0)
                    {
                        /*
                        currentX += 1;
                        map[currentX, currentY] = new RoomData(RoomType.LeftRightBottomTop);
                        */
                        lastDirection = Direction.Down;
                        prevY = currentY;
                        currentY -= 1;
                        map[currentX, currentY] = new RoomData(RoomType.LeftRightBottomTop);
                    }
                    else
                    {
                        done = true;

                    }
                }
            } else if(r < 4) //Move Right
            {
                if (currentX != sizeX- 1 && lastDirection != Direction.Left)
                {
                    lastDirection = Direction.Right;
                    prevX = currentX;
                    currentX += 1;
                    map[currentX, currentY] = new RoomData(RoomType.LeftRight);
                }
                else
                {
                    //if (map[currentX, currentY].roomType == RoomType.LeftRight)
                        map[currentX, currentY].roomType = RoomType.LeftRightBottomTop;

                    if (currentY != 0)
                    {
                        /*
                        currentX -= 1;
                        map[currentX, currentY] = new RoomData(RoomType.LeftRightBottomTop);
                        */
                        lastDirection = Direction.Down;
                        prevY = currentY;
                        currentY -= 1;
                        map[currentX, currentY] = new RoomData(RoomType.LeftRightBottomTop);
                    }
                    else
                    {
                        done = true;

                    }
                }
            } else //Move Down
            {
                //if (map[currentX, currentY].roomType == RoomType.LeftRight)
                    map[currentX, currentY].roomType = RoomType.LeftRightBottomTop;

                if (currentY != 0)
                {
                    prevY = currentY;
                    currentY -= 1;
                    map[currentX, currentY] = new RoomData(RoomType.LeftRightBottomTop);
                }
                else
                {
                    done = true;

                }
                lastDirection = Direction.Down;

            }

            /*
            if (currentY < prevY)
            {
                //Debug.Log("PrevY: " + prevY + ", CurrentY: " + currentY);

                map[currentX, currentY].roomType = RoomType.LeftRightBottomTop;
                //map[prevY, prevX].roomType = RoomType.LeftRightBottomTop;

            }
            */



            mainPath.Add(map[currentX, currentY]);
        }
        endRoom = new Vector2i(currentX, currentY);

        return map;
    }

    public static Map GenerateMap(MapData data)
    {
        Map map = new Map(data);
        
        Vector2i startRoom;
        Vector2i endRoom;

        int[] depths = new int[map.MapChunksX];
        for(int i = 0; i < depths.Length; i++)
        {
            depths[i] = data.baseDepth + (Random.Range(-data.depthVariance, data.depthVariance));
        }
        int randomX = Random.Range((map.MapChunksX/4), map.MapChunksX-(map.MapChunksX/4));
        startRoom = new Vector2i(randomX, depths[randomX]);

        RoomData[,] roomPath = RoomMap(map.MapChunksX, map.MapChunksY, startRoom, out endRoom, data.baseDepth);
        SurfaceLayer temp;
        for (int x = 0; x < map.MapChunksX; x++)
        {
            for (int y = 0; y < map.MapChunksY; y++)
            {
                if (data.mapType == MapType.Hub)
                {
                    map.rooms[x, y] = RoomDatabase.GetRoom(RoomType.Hub);
                    continue;
                }

                if (y < depths[x])
                {
                    temp = SurfaceLayer.Inner;
                } else if(y > depths[x])
                {
                    temp = SurfaceLayer.Above;
                }
                else 
                {
                    temp = SurfaceLayer.Surface;
                }

                if (roomPath[x, y] != null)
                {
                    map.rooms[x, y] = RoomDatabase.GetRoom(roomPath[x, y].roomType, temp);
                }
                else
                {
                    map.rooms[x, y] = RoomDatabase.GetRoom(RoomType.SideRoom, temp);
                }

                
            }
        }

        

        AddBounds(map);


        map.startTile = GetStartTile(map, startRoom.x, startRoom.y);
        Vector2i door = AddDoorTile(map, endRoom.x, endRoom.y);
        //Debug.Log("Door tile " + door);

        map.SetTile(door.x, door.y, TileType.Door);


        if(data.mapType == MapType.Hub)
        {
            map.AddEntity(new EnemyData(5, 5, EnemyType.Slime));
            map.AddEntity(new EnemyData(6, 6, EnemyType.Roller));

        }

        if (data.mapType != MapType.Hub)
        {

            //PopulateMap(map);


            //PopulateBoss(map);
        }

        //Post process the map based on probabilistic tiles and such
        PostProcessing(map);

        AddFallingRock(map);

        return map;
    }

    static void PostProcessing(Map map)
    {
        int enemyRandom = 0;
        int blockRandom = 0;

        for (int x = 0; x < map.sizeX; x++)
        {
            for (int y = 0; y < map.sizeY; y++)
            {
                if(map.GetTile(x, y) == TileType.SmallEnemy)
                {
                    Debug.Log("SmallEnemy");
                    enemyRandom = Random.Range(0, (int)EnemyType.Treedude);
                    map.AddEntity(new EnemyData(x, y, (EnemyType)enemyRandom));
                    map.SetTile(x, y, TileType.Empty);
                } else if(map.GetTile(x, y) == TileType.LargeEnemy)
                {
                    Debug.Log("LargeEnemy");
                    map.AddEntity(new EnemyData(x, y, EnemyType.Treedude));
                    map.SetTile(x, y, TileType.Empty);
                }
                else if (map.GetTile(x, y) == TileType.ObstacleBlock1)
                {
                    //Debug.Log("Obstacle1");
                    blockRandom = Random.Range(0, 3); //This is between empty and bounce
                    map.SetTile(x, y, (TileType)blockRandom);
                }
            }
        }

    }

    static void PopulateBoss(Map map)
    {
        map.AddEntity(new EnemyData(10, 5, EnemyType.LavaBoss));
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
