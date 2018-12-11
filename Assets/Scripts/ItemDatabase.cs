using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabase {

    static ItemObject[] mItemDatabase;

    public static bool InitializeDatabase()
    {
        mItemDatabase = Resources.LoadAll<ItemObject>("Prefabs/Items");
        return true;
    }

    public static ItemObject GetRandomItem()
    {
        return mItemDatabase[Random.Range(0, mItemDatabase.Length)];
    }

   
}
