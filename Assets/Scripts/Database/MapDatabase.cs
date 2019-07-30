using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapDatabase
{

    static MapData[] mMapDatabase;
    static int currentMapIndex = 0;

    public static bool InitializeDatabase()
    {
        mMapDatabase = Resources.LoadAll<MapData>("Maps");
        return true;
    }

    public static MapData GetMap(WorldType worldType)
    {
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

    public static MapData GetBossMap(MapType type, WorldType worldType)
    {
        List<MapData> maplist = new List<MapData>();
        foreach (MapData data in mMapDatabase)
        {
            data.type = worldType;
            if (data.mapType == type)
            {
                maplist.Add(data);
            }
        }

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

    public static MapData GetNextMap(MapType type)
    {
        List<MapData> maplist = new List<MapData>();
        foreach (MapData data in mMapDatabase)
        {
            if (data.mapType == type)
            {
                maplist.Add(data);
            }
        }

        MapData map = maplist[currentMapIndex];

        currentMapIndex++;

        if(currentMapIndex >= maplist.Count)
        {
            currentMapIndex = 0;
        }

        

        return map;

    }
}
