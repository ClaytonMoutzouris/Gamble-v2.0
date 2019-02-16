using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class RoomDatabase
{

    static RoomData[] RoomDictionary;

    public static void InitializeDatabase()
    {

        string[] paths = Directory.GetFiles(Application.dataPath + "/Rooms", "*.room", SearchOption.AllDirectories);
        RoomDictionary = new RoomData[paths.Length];

        for (int i = 0; i < paths.Length; i++)
        {
            RoomData temp = new RoomData();

            //string path = Path.Combine(Application.persistentDataPath, "test.map");
            using (BinaryReader reader = new BinaryReader(File.OpenRead(paths[i])))
            {
                int header = reader.ReadInt32();
                //Debug.Log("Reading room with header " + header);
                if (header == 1)
                {
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
            Debug.Log("Looking for " + roomType + " " + layer);
            Debug.Log("Checking " + data.roomType + " " + data.surfaceLayer);
            if (data.surfaceLayer == layer && data.roomType == roomType)
            {
                roomList.Add(data);
            }
        }
        Debug.Log("Looking for " + roomType + " " + layer + " and found " + roomList.Count);
        return roomList[Random.Range(0, roomList.Count)].Copy();
    }

}
