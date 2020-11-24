using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class RoomDatabase
{

    static RoomData[] RoomDictionary;
    public static string path;

    public static void InitializeDatabase()
    {
        path = Application.dataPath + "/RoomsWorkingDir2/";
        List<RoomData> rooms = FindRooms();

        RoomDictionary = new RoomData[rooms.Count];

        for (int i = 0; i < rooms.Count; i++)
        {
            RoomDictionary[i] = rooms[i].Copy();
        }
    }

    public static List<RoomData> FindRooms()
    {
        List<RoomData> list = new List<RoomData>();

        foreach (string file in Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories))
        {
            Debug.Log("File found - " + file);
            FileStream stream = new FileStream(file, FileMode.OpenOrCreate);

            if (File.Exists(file) && stream.Length > 0)
            {

                BinaryFormatter bf = new BinaryFormatter();

                RoomData data = bf.Deserialize(stream) as RoomData;


                list.Add(data);

            }

            stream.Close();

        }

        return list;
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
            if (data.roomType == RoomType.BossRoom && data.worldType == type)
            {
                roomList.Add(data);
            }
        }

        return roomList[Random.Range(0, roomList.Count)].Copy();
    }

    public static RoomData GetWorldHub(WorldType type)
    {
        List<RoomData> roomList = new List<RoomData>();
        foreach (RoomData data in RoomDictionary)
        {
            if (data.roomType == RoomType.Hub && data.worldType == type)
            {
                roomList.Add(data);
            }
        }

        return roomList[Random.Range(0, roomList.Count)].Copy();
    }

}
