using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyDatabase
{

    static Dictionary<EnemyType, Enemy> mEnemyDatabase;
    static Dictionary<BossType, BossEnemy> mBossDatabase;


    public static bool InitializeDatabase()
    {
        mEnemyDatabase = new Dictionary<EnemyType, Enemy>();
        foreach (Enemy obj in Resources.LoadAll<Enemy>("Prefabs/Enemies"))
        {
            mEnemyDatabase.Add(obj.mEnemyType, obj);
        }

        mBossDatabase = new Dictionary<BossType, BossEnemy>();
        foreach (BossEnemy obj in Resources.LoadAll<BossEnemy>("Prefabs/Bosses"))
        {
            mBossDatabase.Add(obj.bossType, obj);
        }

        return true;
    }

    public static Enemy GetEnemyPrefab(EnemyType id)
    {

        return mEnemyDatabase[id];

    }

    public static BossEnemy GetBossPrefab(BossType id)
    {

        return mBossDatabase[id];

    }


}
