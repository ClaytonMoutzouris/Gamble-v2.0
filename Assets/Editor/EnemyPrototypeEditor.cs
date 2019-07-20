using UnityEditor;
using UnityEngine;

public class EnemyPrototypeEditor
{
    [MenuItem("Assets/Create/Enemy Prototype")]
    public static void CreateEnemyPrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<EnemyPrototype>();
    }

    [MenuItem("Assets/Create/Boss Prototype")]
    public static void CreateBossPrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<BossPrototype>();
    }
}
