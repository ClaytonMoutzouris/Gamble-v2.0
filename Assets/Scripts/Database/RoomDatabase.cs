using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class RoomDatabase
{

    static RoomData[] RoomDictionary;

    public static void InitializeDatabase()
    {

        Object[] objects = Resources.LoadAll("Rooms");
        List<TextAsset> assets = new List<TextAsset>();
        foreach (Object o in objects)
        {
            assets.Add((TextAsset)o);
        }

        Debug.Log(assets == null);
        RoomDictionary = new RoomData[assets.Count];

        for (int i = 0; i < assets.Count; i++)
        {
            RoomData temp = new RoomData();

            //string path = Path.Combine(Application.persistentDataPath, "test.map");

            Stream s = new MemoryStream(assets[i].bytes);
            using (BinaryReader reader = new BinaryReader(s))
            {
                int header = reader.ReadInt32();
                //Debug.Log("Reading room with header " + header);
                if (header == 1)
                {
                    Debug.LogWarning("Loading a room ");

                    temp.Load(reader);
                }
                else
                {
                    Debug.LogWarning("Unknown room format " + header);
                }

            }
            RoomDictionary[i] = temp.Copy();
        }
    }
    /*
    public static bool InitializeDatabase()
    {
        RoomDictionary = Resources.LoadAll<RoomData>("Prototypes/Rooms");
        return true;
    }
    */
    public static RoomData GetRoom(RoomType roomType)
    {
        List<RoomData> roomList = new List<RoomData>();
        foreach (RoomData data in RoomDictionary)
        {
            if (data.roomType == roomType)
            {
                roomList.Add(data);
            }
        }

        return roomList[Random.Range(0, roomList.Count)].Copy();
    }

    public static RoomData GetRoom(SurfaceLayer layer)
    {
        List<RoomData> roomList = new List<RoomData>();
        foreach (RoomData data in RoomDictionary)
        {
            if (data.surfaceLayer == layer)
            {
                roomList.Add(data);
            }
        }

        return roomList[Random.Range(0, roomList.Count)].Copy();
    }

    public static RoomData GetRoom(RoomType roomType, SurfaceLayer layer)
    {
        List<RoomData> roomList = new List<RoomData>();
        foreach (RoomData data in RoomDictionary)
        {
            if (data.surfaceLayer == layer && data.roomType == roomType)
            {
                roomList.Add(data);
            }
        }
        return roomList[Random.Range(0, roomList.Count)].Copy();
    }

    public static RoomData GetBossRoom(WorldType type)
    {
        List<RoomData> roomList = new List<RoomData>();
        foreach (RoomData data in RoomDictionary)
        {
            if (data.roomType == RoomType.BossRoom)
            {
                roomList.Add(data);
            }
        }

        return roomList[Random.Range(0, roomList.Count)].Copy();
    }

}
