using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyDatabase
{

    static Dictionary<EnemyType, Enemy> mEnemyDatabase;
    static Dictionary<BossType, Enemy> mBossDatabase;


    public static bool InitializeDatabase()
    {
        mEnemyDatabase = new Dictionary<EnemyType, Enemy>();
        foreach (Enemy obj in Resources.LoadAll<Enemy>("Prefabs/Enemies"))
        {
            mEnemyDatabase.Add(obj.mEnemyType, obj);
        }

        mBossDatabase = new Dictionary<BossType, Enemy>();
        foreach (Enemy obj in Resources.LoadAll<Enemy>("Prefabs/Bosses"))
        {
            mBossDatabase.Add(obj.bossType, obj);
        }

        return true;
    }

    public static Enemy GetEnemyPrefab(EnemyType id)
    {

        return mEnemyDatabase[id];

    }

    public static Enemy GetBossPrefab(BossType id)
    {

        return mBossDatabase[id];

    }


}
