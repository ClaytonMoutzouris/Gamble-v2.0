﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    List<Map> maps;
    int currentMapIndex = 0;
    WorldType worldType;
    //Reference to this worlds node
    WorldNode worldNode;
    public bool worldCleared = false;


    public World(WorldData data)
    {
        WorldType = data.type;
        maps = new List<Map>();

        foreach(MapData mapData in data.maps)
        {
            mapData.tileSprites = data.tileSprites;
            mapData.gravity = data.gravity;

            switch(mapData.mapType)
            {
                case MapType.World:
                    maps.Add(MapGenerator.GenerateMap(mapData));
                    break;
                case MapType.BossMap:
                    maps.Add(MapGenerator.GenerateBossMap(mapData));
                    break;
                case MapType.Hub:
                    maps.Add(MapGenerator.GenerateHubMap(mapData));
                    break;
            }
        }

    }

    public WorldType WorldType { get => worldType; set => worldType = value; }
    public WorldNode WorldNode { get => worldNode; set => worldNode = value; }

    public Map GetNextMap()
    {
        Map temp = null;

        if (currentMapIndex < maps.Count)
        {
            temp = maps[currentMapIndex];
        }
        else
        {
            temp = WorldManager.instance.GetHubWorld().GetFirstMap();
        }

        currentMapIndex++;

        return temp;
    }

    public Map GetFirstMap()
    {
        return maps[0];
    }

    public void WorldCleared()
    {
        worldCleared = true;
        WorldNode.SetCleared();
    }

    public bool IsCleared()
    {
        return worldCleared;
    }
}