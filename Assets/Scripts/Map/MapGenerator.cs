using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using AccidentalNoise;


public enum Direction { Up, Left, Down, Right, Count };


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

        for (int y = 1; y < map.roomSizeX; y++)
        {
            for (int x = 0; x < map.roomSizeY; x++)
            {

                if (map.GetTile(roomX*map.roomSizeX + x, roomY * map.roomSizeY + y) == TileType.Empty)
                {
                    if (map.GetTile(roomX * map.roomSizeX + x, roomY * map.roomSizeY + y - 1) == TileType.Block)
                    {
                        possibleTiles.Add(new Vector2i(roomX * map.roomSizeX + x, roomY * map.roomSizeY + y));
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

        for (int y = 1; y < map.roomSizeX; y++)
        {
            for (int x = 0; x < map.roomSizeY; x++)
            {

                if (map.GetTile(roomX * map.roomSizeX + x, roomY * map.roomSizeY + y) == TileType.Empty)
                {
                    if (map.GetTile(roomX * map.roomSizeX + x, roomY * map.roomSizeY + y - 1) == TileType.Block)
                    {
                        possibleTiles.Add(new Vector2i(roomX * map.roomSizeX + x, roomY * map.roomSizeY + y));
                    }
                }
            }
        }

        tile = possibleTiles[Random.Range(0, possibleTiles.Count)];

        return tile;

    }

    public static void AddBounds(Map map)
    {

        for (int j = 0; j < map.getMapSize().x; j++)
        {

            map.SetTile(j, 0, TileType.Block);
            
        }

    }

    public static RoomData[,] RoomMap(int sizeX, int sizeY, Vector2i startRoom, out Vector2i endRoom)
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
        Direction lastDirection = Direction.Left;

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
        mainPath[mainPath.Count-1].roomType = RoomType.LeftRightBottomTop;
        return map;
    }

    public static Map GenerateMap(MapData data)
    {


        Map map = new Map(data);
        RoomData[,] rooms = new RoomData[data.sizeX, data.sizeY];

        Vector2i startRoom;
        Vector2i endRoom;
        int randomX = Random.Range(0, map.sizeX);

        int[] depths = new int[map.sizeX];


        if (data.baseDepth < data.sizeY)
        {
            for (int i = 0; i < depths.Length; i++)
            {
                depths[i] = data.baseDepth + (Random.Range(-data.depthVariance, data.depthVariance));
            }
            Debug.Log("Random X: " + randomX);
            startRoom = new Vector2i(randomX, depths[randomX]);
        }
        else
        {
            for (int i = 0; i < depths.Length; i++)
            {
                depths[i] = data.sizeY;
            }

            startRoom = new Vector2i(randomX, map.sizeY-1);
        }

        RoomData[,] roomPath = RoomMap(map.sizeX, map.sizeY, startRoom, out endRoom);

        if(data.hasMiniboss)
        {
            Debug.Log("Has miniboss");
            roomPath[endRoom.x, endRoom.y].roomType = RoomType.MinibossRoom;
        }
        SurfaceLayer temp;

        for (int x = 0; x < map.sizeX; x++)
        {
            for (int y = 0; y < map.sizeY; y++)
            {

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
                    if (roomPath[x, y].roomType == RoomType.MinibossRoom)
                    {
                        rooms[x, y] = RoomDatabase.GetRoom(RoomType.MinibossRoom);

                    } else
                    {
                        rooms[x, y] = RoomDatabase.GetRoom(roomPath[x, y].roomType, temp);
                    }
                }
                else
                {
                    rooms[x, y] = RoomDatabase.GetRoom(RoomType.SideRoom, temp);
                }

                foreach(EntityData eData in rooms[x, y].entityData)
                {
                    if (eData == null)
                        continue;
                    map.AddEntity(eData);
                }
            }
        }

        map.SetTileMap(RoomsToMap(rooms, data.sizeX, data.sizeY));
        
        //TODO Turn room array into map



        AddBounds(map);

        Debug.Log("Start room: " + startRoom.x + ", " + startRoom.y);
        Vector2i door = AddDoorTile(map, endRoom.x, endRoom.y);
        map.SetTile(door.x, door.y, TileType.Door);
        map.exitTile = door;
        //Debug.Log("Door tile " + door);
        map.startTile = GetStartTile(map, startRoom.x, startRoom.y);


        if (data.mapType != MapType.Ship && data.mapType != MapType.BossMap)
        {

            //PopulateMap(map);

            //PopulateBoss(map);
            AddChests(map);
            //AddFauna(map);
            //AddFallingRock(map);

            //PopulateBoss(map);
        }


        //Post process the map based on probabilistic tiles and such
        PostProcessing(map, data);


        return map;
    }

    public static TileType[,] RoomsToMap(RoomData[,] rooms, int numRoomsX, int numRoomsY)
    {
        int mapsizeX = Constants.cRoomSizeX * numRoomsX;
        int mapSizeY = Constants.cRoomSizeY * numRoomsY;

        TileType[,] tileMap = new TileType[mapsizeX, mapSizeY];
        
        for(int rx = 0; rx < numRoomsX; rx++)
        {
            for(int ry = 0; ry < numRoomsY; ry++)
            {
                for(int tx = 0; tx < Constants.cRoomSizeX; tx++)
                 {
                    for(int ty = 0; ty < Constants.cRoomSizeY; ty++)
                    {
                        tileMap[rx * Constants.cRoomSizeX + tx, ry * Constants.cRoomSizeY + ty] = rooms[rx, ry].tiles[tx, ty];
                    }
                }
            }
        }


        return tileMap;
    }

    public static Map GenerateBossMap(MapData data)
    {

        RoomData roomData = RoomDatabase.GetBossRoom(data.type);
        data.roomSizeX = roomData.mWidth;
        data.roomSizeY = roomData.mHeight;
        Map map = new Map(data);
        map.SetTileMap(roomData.tiles);
        Debug.Log("Rooms Size: " + map.getMapSize().x + ", " + map.getMapSize().y);

        AddBounds(map);

        bool hasDoor = false;

        for (int x = 0; x < map.getMapSize().x; x++)
        {
            for (int y = 0; y < map.getMapSize().y; y++)
            {
              if(map.GetTile(x,y) == TileType.Door)
                {
                    hasDoor = true;
                    map.exitTile = new Vector2i(x,y);

                }
            }
        } 

        if(!hasDoor)
        {
            //map.startTile = GetStartTile(map, 0, 0);
            Vector2i door = AddDoorTile(map, 0, 0);
            map.exitTile = door;
            //Debug.Log("Door tile " + door);

            map.SetTile(door.x, door.y, TileType.Door);
        }

        foreach (EntityData eData in roomData.entityData)
        {
            if (eData == null)
                continue;
            map.AddEntity(eData);
        }


        map.bossTile = new Vector2i(8, 2);

        //This needs a +1 to hit all the bosses
        map.AddEntity(new BossData(map.bossTile.x, map.bossTile.y, GetBossForWorld(map.worldType)));
        //Post process the map based on probabilistic tiles and such
        PostProcessing(map, data);
        return map;
    }

    public static Map GenerateShipMap(MapData data)
    {
        Debug.Log("Rooms Size: " + data.roomSizeX + ", " + data.roomSizeY);

        RoomData roomData = RoomDatabase.GetRoom(RoomType.Ship);
        data.roomSizeX = roomData.mWidth;
        data.roomSizeY = roomData.mHeight;

        Debug.Log("Rooms Size: " + data.roomSizeX + ", " + data.roomSizeY);
        Map map = new Map(data);



        map.SetTileMap(roomData.tiles);
        Debug.Log("Rooms Size: " + map.getMapSize().x + ", " + map.getMapSize().y);

        AddBounds(map);
        /*
        map.AddEntity(new NPCData(1, 1, NPCType.ArmsDealer));
        map.AddEntity(new NPCData(14, 1, NPCType.GeneralTrader));
        map.AddEntity(new NPCData(8, 4, NPCType.Gadgeteer));
        map.AddEntity(new NPCData(20, 1, NPCType.PotionSeller));
        map.AddEntity(new NPCData(22, 4, NPCType.Clothier));
        map.AddEntity(new NPCData(10, 13, NPCType.EggsDealer));
        */
        //map.AddEntity(new BossData(10, 13, BossType.VoidBoss));


        if (WorldManager.instance.NumCompletedWorlds() == 0)
        {
            map.AddEntity(new ItemObjectData(6, 1, ObjectType.Item, "Biosample"));
            map.AddEntity(new ItemObjectData(7, 1, ObjectType.Item, "Slime Egg"));


        }

        if (WorldManager.instance.NumCompletedWorlds() == 0)
        {
            //map.AddEntity(new ItemObjectData(6, 1, ObjectType.Item, "Snowzooka"));

        }
        //map.AddEntity(new MinibossData(15, 10, map.Data.minibossType));
        //map.AddEntity(new EnemyData(15, 10, EnemyType.Sporby));

        if (WorldManager.instance.NumCompletedWorlds() == 0)
        {
            /*
            map.AddEntity(new ItemObjectData(8, 1, ObjectType.Item, "ForceFieldGadget"));
            map.AddEntity(new ItemObjectData(9, 1, ObjectType.Item, "NovaGadget"));
            map.AddEntity(new ItemObjectData(10, 1, ObjectType.Item, "HookShot"));
            map.AddEntity(new ItemObjectData(10, 1, ObjectType.Item, "HookShot"));
            map.AddEntity(new ItemObjectData(11, 1, ObjectType.Item, "PortalGadget"));
            map.AddEntity(new ItemObjectData(12, 1, ObjectType.Item, "Teleporter"));
            map.AddEntity(new ItemObjectData(13, 1, ObjectType.Item, "CaptureGadget"));
            */

        }

        foreach (EntityData eData in roomData.entityData)
        {
            if (eData == null)
                continue;
            map.AddEntity(eData);
        }
        //Post process the map based on probabilistic tiles and such
        PostProcessing(map, data);

        return map;
    }

    public static Map GenerateHubMap(MapData data)
    {
        Debug.Log("Rooms Size: " + data.roomSizeX + ", " + data.roomSizeY);

        RoomData roomData = RoomDatabase.GetRoom(RoomType.Hub);
        data.roomSizeX = roomData.mWidth;
        data.roomSizeY = roomData.mHeight;

        Debug.Log("Rooms Size: " + data.roomSizeX + ", " + data.roomSizeY);
        Map map = new Map(data);



        map.SetTileMap(roomData.tiles);
        Debug.Log("Rooms Size: " + map.getMapSize().x + ", " + map.getMapSize().y);

        AddBounds(map);


        map.AddEntity(new ObjectData(12, 1, ObjectType.Medbay));
        map.AddEntity(new ObjectData(8, 1, ObjectType.SaveMachine));

        foreach (EntityData eData in roomData.entityData)
        {
            if (eData == null)
                continue;
            map.AddEntity(eData);
        }
        //Post process the map based on probabilistic tiles and such
        PostProcessing(map, data);

        return map;
    }

    public static BossType GetBossForWorld(WorldType world)
    {
        BossType temp = BossType.Count;
        switch (world)
        {
            case WorldType.Forest:
                temp = BossType.HedgehogBoss;
                break;
            case WorldType.Tundra:
                temp = BossType.CatBoss;
                break;
            case WorldType.Lava:
                temp = BossType.LavaBoss;
                break;
            case WorldType.Purple:
                temp = BossType.SharkBoss;
                break;
            case WorldType.Yellow:
                temp = BossType.TentacleBoss;
                break;
            case WorldType.Void:
                temp = BossType.VoidBoss;
                break;
        }
        return temp;
    }

    public static Map CreationMap()
    {
        Map map = new Map(MapType.Ship, WorldType.Ship, 20, 20);



        AddBounds(map);


        map.startTile = GetStartTile(map, 0, 0);
        Vector2i door = AddDoorTile(map, 0, 0);
        map.exitTile = door;
        //Debug.Log("Door tile " + door);

        map.SetTile(door.x, door.y, TileType.Door);


        return map;

    }

    static void PostProcessing(Map map, MapData data)
    {
        int enemyRandom = 0;
        int blockRandom = 0;

        for (int x = 0; x < map.getMapSize().x; x++)
        {
            for (int y = 0; y < map.getMapSize().y; y++)
            {
                /*
                if (map.GetTile(x, y) == TileType.Empty)
                {
                    int r;
                    r = Random.Range(0, 100);
                    if (r < 50)
                        continue;

                    if (y != 0 && map.GetTile(x, y - 1) == TileType.Block)
                        map.AddEntity(new ObjectData(x, y, ObjectType.FlowerBed));
                }
                */
                switch (map.GetTile(x, y))
                {
                    case TileType.SmallEnemy:
                        enemyRandom = Random.Range(0, data.smallEnemies.Count);
                        map.AddEntity(new EnemyData(x, y, data.smallEnemies[enemyRandom]));
                        map.SetTile(x, y, TileType.Empty);
                        break;
                    case TileType.LargeEnemy:
                        enemyRandom = Random.Range(0, data.largeEnemies.Count);
                        map.AddEntity(new EnemyData(x, y, data.largeEnemies[enemyRandom]));
                        map.SetTile(x, y, TileType.Empty);
                        break;
                    case TileType.ObstacleBlock1:
                        //Debug.Log("Obstacle1");
                        blockRandom = Random.Range(0, 3); //This is between empty and bounce
                        map.SetTile(x, y, (TileType)blockRandom);
                        break;
                    case TileType.FallingRock:
                        map.AddEntity(new ObjectData(x, y, ObjectType.FallingRock));
                        map.SetTile(x, y, TileType.Empty);
                        break;
                    case TileType.Chest:
                        map.AddEntity(new ObjectData(x, y, ObjectType.Chest));
                        map.SetTile(x, y, TileType.Empty);
                        break;
                    case TileType.FlowerBed:
                        map.AddEntity(new ObjectData(x, y, ObjectType.FlowerBed));
                        map.SetTile(x, y, TileType.Empty);
                        break;
                    case TileType.StartTile:
                        map.startTile = new Vector2i(x, y);
                        map.SetTile(x, y, TileType.Empty);
                        break;
                    case TileType.Door:
                        map.AddEntity(new ObjectData(x, y, ObjectType.Door));
                        map.SetTile(x, y, TileType.Empty);
                        break;
                    case TileType.Bounce:
                        //map.AddEntity(new ObjectData(x, y, ObjectType.BouncePad));
                        //map.SetTile(x, y, TileType.Empty);
                        break;
                    case TileType.Spikes:
                        //map.AddEntity(new ObjectData(x, y, ObjectType.Spikes));
                        //map.SetTile(x, y, TileType.Empty);
                        break;
                    case TileType.MinibossSpawn:
                        map.AddEntity(new MinibossData(x, y, map.Data.minibossType));
                        map.SetTile(x, y, TileType.Empty);
                        break;
                    case TileType.SmallOre:
                        map.AddEntity(new ObjectData(x, y, ObjectType.SmallGatherable));
                        map.SetTile(x, y, TileType.Empty);
                        break;
                    case TileType.LargeOre:
                        map.AddEntity(new ObjectData(x, y, ObjectType.LargeGatherable));
                        map.SetTile(x, y, TileType.Empty);
                        break;
                    case TileType.Water:
                        if(data.mapType == MapType.Ship || data.mapType == MapType.Hub)
                        {
                            break;
                        }
                        int random = Random.Range(0, 100);
                        if(random >= 95)
                        {
                            map.AddEntity(new EnemyData(x, y, EnemyType.Nipper));
                        }
                        break;
                    case TileType.BreakableTile:
                        map.AddEntity(new ObjectData(x, y, ObjectType.BreakableTile));
                        map.SetTile(x, y, TileType.Empty);
                        break;
                }

            }
        }

    }


    static void PopulateMap(Map map)
    {
        AddChests(map);
        AddFauna(map);
        //AddEnemies(map);
        AddFallingRock(map);



    }
    
    static void AddFauna(Map map)
    {
        Debug.Log("Adding Fauna");
        int r;

        for (int x = 0; x < map.getMapSize().x; x++)
        {
            for (int y = 0; y < map.getMapSize().y; y++)
            {
                r = Random.Range(0, 100);
                if (r < 30)
                    continue;

                if (map.GetTile(x, y) == TileType.Empty)
                {
                    if (y != 0 && map.GetTile(x, y - 1) == TileType.Block)
                        if (r >= 40)
                        {
                            map.AddEntity(new ObjectData(x, y, ObjectType.FlowerBed));
                        }
                        else
                        {
                            map.AddEntity(new ObjectData(x, y, ObjectType.Tree));
                        }
                }
            }
        }
    }
    
    static void AddChests(Map map)
    {
        int r;

        for (int x = 0; x < map.getMapSize().x; x++)
        {
            for (int y = 0; y < map.getMapSize().y; y++)
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

        for (int x = 0; x < map.getMapSize().x; x++)
        {
            for (int y = 0; y < map.getMapSize().y; y++)
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




}
