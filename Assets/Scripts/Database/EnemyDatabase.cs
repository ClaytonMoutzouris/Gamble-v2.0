using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyDatabase
{

    static List<EnemyPrototype> mEnemyDatabase;

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
        EnemyPrototype[] resources = Resources.LoadAll<EnemyPrototype>(@"Prototypes/Entity/Enemies"); // Load all items from the Resources/Items folder
        foreach (EnemyPrototype enemy in resources)
        {
            if (!mEnemyDatabase.Contains(enemy)) // If list doesn't contain item then add it 
            {
                mEnemyDatabase.Add(enemy);
            }
        }
    }

    static private void ValidateDatabase() // Is list null and/or loaded?
    {
        if (mEnemyDatabase == null) mEnemyDatabase = new List<EnemyPrototype>(); // If list is null, create list
        if (!isDatabaseLoaded) LoadDatabase(); // If database is not loaded, load database
    }

    public static EnemyPrototype GetEnemyPrototype(EnemyType id)
    {
        ValidateDatabase();

        foreach (EnemyPrototype enemy in mEnemyDatabase)
        {
            if (enemy.enemyType == id)
            {
                return ScriptableObject.Instantiate(enemy) as EnemyPrototype;
            }
        }
        return null;

    }

}
