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




}
