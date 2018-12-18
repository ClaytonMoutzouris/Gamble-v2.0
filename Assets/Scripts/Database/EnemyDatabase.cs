using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyDatabase
{

    static Dictionary<EnemyType, Enemy> mEnemyDatabase;

    public static bool InitializeDatabase()
    {
        mEnemyDatabase = new Dictionary<EnemyType, Enemy>();
        foreach (Enemy obj in Resources.LoadAll<Enemy>("Prefabs/Enemies"))
        {
            mEnemyDatabase.Add(obj.mEnemyType, obj);
        }

        return true;
    }

    public static Enemy GetEnemyPrefab(EnemyType id)
    {

        return mEnemyDatabase[id];

    }


}
