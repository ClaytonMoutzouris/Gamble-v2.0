using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BossDatabase
{

    static List<BossPrototype> mBossDatabase;

    static private bool isDatabaseLoaded = false;

    static public void LoadDatabase()
    {
        if (isDatabaseLoaded) return;
        isDatabaseLoaded = true;
        LoadDatabaseForce();
    }


    public static void LoadDatabaseForce()
    {
        ValidateDatabase();
        EnemyPrototype[] resources = Resources.LoadAll<BossPrototype>(@"Prototypes/Entity/Bosses"); // Load all items from the Resources/Items folder
        foreach (BossPrototype boss in resources)
        {
            if (!mBossDatabase.Contains(boss)) // If list doesn't contain item then add it 
            {
                mBossDatabase.Add(boss);
            }
        }
    }

    static private void ValidateDatabase() // Is list null and/or loaded?
    {
        if (mBossDatabase == null) mBossDatabase = new List<BossPrototype>(); // If list is null, create list
        if (!isDatabaseLoaded) LoadDatabase(); // If database is not loaded, load database
    }

    public static BossPrototype GetBossPrototype(BossType id)
    {
        ValidateDatabase();

        foreach (BossPrototype boss in mBossDatabase)
        {
            if (boss.bossType == id)
            {
                return ScriptableObject.Instantiate(boss) as BossPrototype;
            }
        }
        return null;

    }

}
