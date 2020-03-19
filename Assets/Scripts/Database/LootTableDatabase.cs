using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LootTableDatabase
{

    static LootTable[] mLootTableDatabase;

    public static bool InitializeDatabase()
    {
        mLootTableDatabase = Resources.LoadAll<LootTable>("LootTables");
        return true;
    }

    public static LootTable GetRandomItem()
    {
        LootTable item = ScriptableObject.Instantiate(mLootTableDatabase[Random.Range(0, mLootTableDatabase.Length)]);

        return item;
    }

}
