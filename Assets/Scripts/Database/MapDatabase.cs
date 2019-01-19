using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapDatabase
{

    static MapData[] mItemDatabase;

    public static bool InitializeDatabase()
    {
        mItemDatabase = Resources.LoadAll<MapData>("Maps");
        return true;
    }


}
