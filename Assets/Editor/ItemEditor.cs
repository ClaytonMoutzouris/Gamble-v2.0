using UnityEditor;
using UnityEngine;

public class ItemEditor
{

    [MenuItem("Assets/Create/Items/LootTable")]
    public static void CreateBaseLootTableAsset()
    {
        ScriptableObjectUtility.CreateAsset<LootTable>();
    }

    [MenuItem("Assets/Create/Items/Weapon/MeleeWeapon")]
    public static void CreateBaseMeleeWeaponAsset()
    {
        ScriptableObjectUtility.CreateAsset<MeleeWeapon>();
    }

    [MenuItem("Assets/Create/Items/Weapon/RangedWeapon")]
    public static void CreateBaseRangedWeaponAsset()
    {
        ScriptableObjectUtility.CreateAsset<RangedWeapon>();
    }

    [MenuItem("Assets/Create/Items/Armor")]
    public static void CreateBaseArmorAsset()
    {
        ScriptableObjectUtility.CreateAsset<Armor>();
    }
    [MenuItem("Assets/Create/Items/Consumable")]
    public static void CreateBaseConsumableAsset()
    {
        ScriptableObjectUtility.CreateAsset<ConsumableItem>();
    }
    [MenuItem("Assets/Create/Items/Consumable/Medkit")]
    public static void CreateMedkitAsset()
    {
        ScriptableObjectUtility.CreateAsset<Medkit>();
    }
    [MenuItem("Assets/Create/Items/Consumable/Food")]
    public static void CreateFoodAsset()
    {
        ScriptableObjectUtility.CreateAsset<Food>();
    }
    [MenuItem("Assets/Create/Items/QuestItem")]
    public static void CreateBaseQuestItemAsset()
    {
        ScriptableObjectUtility.CreateAsset<QuestItem>();
    }

    [MenuItem("Assets/Create/Items/Key")]
    public static void CreateBaseKeyItemAsset()
    {
        ScriptableObjectUtility.CreateAsset<Key>();
    }


}
