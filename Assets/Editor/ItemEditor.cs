using UnityEditor;
using UnityEngine;

public class ItemEditor
{

    [MenuItem("Assets/Create/Items/Weapon")]
    public static void CreateBaseWeaponAsset()
    {
        ScriptableObjectUtility.CreateAsset<Weapon>();
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




}
