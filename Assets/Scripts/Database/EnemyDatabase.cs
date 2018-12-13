using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyDatabase
{

    static Dictionary<EnemyType, EnemyObject> mEnemyDatabase;

    public static bool InitializeDatabase()
    {
        mEnemyDatabase = new Dictionary<EnemyType, EnemyObject>();
        foreach (EnemyObject obj in Resources.LoadAll<EnemyObject>("Prefabs/Enemies"))
        {
            mEnemyDatabase.Add(obj.mEnemyType, obj);
        }

        return true;
    }

    public static EnemyObject GetEnemyPrefab(EnemyType id)
    {

        return mEnemyDatabase[id];

    }


}
