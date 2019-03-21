using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabase {

    static Item[] mItemDatabase;

    public static bool InitializeDatabase()
    {
        mItemDatabase = Resources.LoadAll<Item>("Items");
        return true;
    }

    public static Item GetRandomItem()
    {
        return mItemDatabase[Random.Range(0, mItemDatabase.Length)];
    }

    
   
}
