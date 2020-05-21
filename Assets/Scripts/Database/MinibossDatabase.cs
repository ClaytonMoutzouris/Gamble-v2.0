using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MinibossDatabase
{

    static List<MinibossPrototype> mMinibossDatabase;
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
        EnemyPrototype[] resources = Resources.LoadAll<MinibossPrototype>(@"Prototypes/Entity/Minibosses"); // Load all items from the Resources/Items folder
        foreach (MinibossPrototype boss in resources)
        {
            if (!mMinibossDatabase.Contains(boss)) // If list doesn't contain item then add it 
            {
                mMinibossDatabase.Add(boss);
            }
        }
    }

    static private void ValidateDatabase() // Is list null and/or loaded?
    {
        if (mMinibossDatabase == null) mMinibossDatabase = new List<MinibossPrototype>(); // If list is null, create list
        if (!isDatabaseLoaded) LoadDatabase(); // If database is not loaded, load database
    }

    public static MinibossPrototype GetMinibossPrototype(MinibossType id)
    {
        ValidateDatabase();

        foreach (MinibossPrototype miniboss in mMinibossDatabase)
        {
            if (miniboss.minibossType == id)
            {
                return ScriptableObject.Instantiate(miniboss) as MinibossPrototype;
            }
        }
        return null;

    }

}
