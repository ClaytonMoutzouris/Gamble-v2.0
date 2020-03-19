using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapDatabase
{

    static MapData[] mMapDatabase;
    static MapData[] mBossMapDatabase;
    static MapData hubMap;
    static int currentMapIndex = 0;

    public static bool InitializeDatabase()
    {
        mMapDatabase = Resources.LoadAll<MapData>("Maps/WorldMaps");
        mBossMapDatabase = Resources.LoadAll<MapData>("Maps/BossMaps");
        hubMap = Resources.Load<MapData>("Maps/HubMap");

        return true;
    }

    public static MapData GetHubMap()
    {
        return hubMap;
    }

    public static MapData GetMap(WorldType worldType)
    {
        if(worldType == WorldType.Hub)
        {
            return hubMap;
        }

        List<MapData> maplist = new List<MapData>();
        foreach (MapData data in mMapDatabase)
        {
            if(data.type == worldType)
            {
                maplist.Add(data);
            }
        }
        return maplist[Random.Range(0, maplist.Count)];
    }

    public static MapData GetBossMap(WorldType type)
    {
        List<MapData> maplist = new List<MapData>();
        foreach (MapData data in mBossMapDatabase)
        {
            if (data.type == type)
            {
                maplist.Add(data);
            }
        }
        Debug.Log("Getting boss with world type" + type);

        return maplist[Random.Range(0, maplist.Count)];
    }

    public static MapData GetMap(MapType type)
    {
        List<MapData> maplist = new List<MapData>();
        foreach (MapData data in mMapDatabase)
        {
            if (data.mapType == type)
            {
                maplist.Add(data);
            }
        }

        return maplist[Random.Range(0, maplist.Count)];
    }

    
}
