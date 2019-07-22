using UnityEditor;
using UnityEngine;

public class EntityPrototypeEditor
{
    [MenuItem("Assets/Create/Entity/Player")]
    public static void CreatePlayerPrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<PlayerPrototype>();
    }

    [MenuItem("Assets/Create/Entity/Enemy")]
    public static void CreateEnemyPrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<EnemyPrototype>();
    }

    [MenuItem("Assets/Create/Entity/Boss")]
    public static void CreateBossPrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<BossPrototype>();
    }

    [MenuItem("Assets/Create/Entity/Entity")]
    public static void CreateEntityPrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<EntityPrototype>();
    }
}
