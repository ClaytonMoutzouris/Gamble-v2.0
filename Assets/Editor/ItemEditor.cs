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

    [MenuItem("Assets/Create/Items/Biosample")]
    public static void CreateBaseBiosampleAsset()
    {
        ScriptableObjectUtility.CreateAsset<Biosample>();
    }

    [MenuItem("Assets/Create/Items/Element")]
    public static void CreateBaseElementAsset()
    {
        ScriptableObjectUtility.CreateAsset<Element>();
    }

    [MenuItem("Assets/Create/Items/Gadget")]
    public static void CreateBaseGadgetAsset()
    {
        ScriptableObjectUtility.CreateAsset<Gadget>();
    }

    [MenuItem("Assets/Create/Items/Gadget/Teleporter")]
    public static void CreateBaseTeleporterAsset()
    {
        ScriptableObjectUtility.CreateAsset<Teleporter>();
    }

    [MenuItem("Assets/Create/Items/Gadget/NovaGadget")]
    public static void CreateBaseNovaGadgetAsset()
    {
        ScriptableObjectUtility.CreateAsset<AttackGadget>();
    }

    [MenuItem("Assets/Create/Items/Gadget/PortalGadget")]
    public static void CreateBasePortalGadgetAsset()
    {
        ScriptableObjectUtility.CreateAsset<PortalGadget>();
    }

    [MenuItem("Assets/Create/Items/Gadget/HookShot")]
    public static void CreateBaseHookShotAsset()
    {
        ScriptableObjectUtility.CreateAsset<HookShot>();
    }

    [MenuItem("Assets/Create/Items/Gadget/ForceFieldGadget")]
    public static void CreateBaseForceFieldGadgetAsset()
    {
        ScriptableObjectUtility.CreateAsset<ForceFieldGadget>();
    }

    [MenuItem("Assets/Create/Items/Gadget/CaptureGadget")]
    public static void CreateBaseCaptureGadgetAsset()
    {
        ScriptableObjectUtility.CreateAsset<CaptureGadget>();
    }

    [MenuItem("Assets/Create/Items/Gadget/GravityRemote")]
    public static void CreateBaseGravityRemoteAsset()
    {
        ScriptableObjectUtility.CreateAsset<GravityRemote>();
    }
}
