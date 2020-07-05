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

    [MenuItem("Assets/Create/Entity/Miniboss")]
    public static void CreateMinibossPrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<MinibossPrototype>();
    }

    [MenuItem("Assets/Create/Entity/Entity")]
    public static void CreateEntityPrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<EntityPrototype>();
    }

    [MenuItem("Assets/Create/Entity/Door")]
    public static void CreateDoorPrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<DoorPrototype>();
    }

    [MenuItem("Assets/Create/Entity/Chest")]
    public static void CreateChestPrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<ChestPrototype>();
    }

    [MenuItem("Assets/Create/Entity/Gatherable")]
    public static void CreateGatherablePrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<GatherablePrototype>();
    }

    [MenuItem("Assets/Create/Entity/NPC")]
    public static void CreateNPCPrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<NPCPrototype>();
    }

    [MenuItem("Assets/Create/Entity/Drone")]
    public static void CreateDronePrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<DronePrototype>();
    }
}
