using UnityEditor;
using UnityEngine;

public class AbilityEditor
{

    [MenuItem("Assets/Create/Ability/StatusOnHit")]
    public static void CreateBaseStatusOnHitAsset()
    {
        ScriptableObjectUtility.CreateAsset<StatusOnHit>();
    }

    [MenuItem("Assets/Create/Ability/Aura")]
    public static void CreateBaseAuraAsset()
    {
        ScriptableObjectUtility.CreateAsset<Aura>();
    }

    [MenuItem("Assets/Create/Ability/DamageReflect")]
    public static void CreateBaseDamageReflectAsset()
    {
        ScriptableObjectUtility.CreateAsset<DamageReflect>();
    }

    [MenuItem("Assets/Create/Ability/ExtraJump")]
    public static void CreateBaseExtraJumpAsset()
    {
        ScriptableObjectUtility.CreateAsset<ExtraJump>();
    }

    [MenuItem("Assets/Create/Ability/Lifesteal")]
    public static void CreateBaseLifestealAsset()
    {
        ScriptableObjectUtility.CreateAsset<Lifesteal>();
    }

    [MenuItem("Assets/Create/Ability/SetAbilityFlag")]
    public static void CreateBaseSetAbilityFlagAsset()
    {
        ScriptableObjectUtility.CreateAsset<SetAbilityFlag>();
    }

    [MenuItem("Assets/Create/Ability/SpawnAttackOnWalk")]
    public static void CreateBaseSpawnAttackOnWalkAsset()
    {
        ScriptableObjectUtility.CreateAsset<SpawnAttackOnWalk>();
    }

    [MenuItem("Assets/Create/Ability/StatBuff")]
    public static void CreateBaseStatBuffAsset()
    {
        ScriptableObjectUtility.CreateAsset<StatBuff>();
    }

    [MenuItem("Assets/Create/Ability/ChestFinder")]
    public static void CreateBaseChestFinderAsset()
    {
        ScriptableObjectUtility.CreateAsset<ChestFinder>();
    }

    [MenuItem("Assets/Create/Ability/HealSharing")]
    public static void CreateBaseHealSharingAsset()
    {
        ScriptableObjectUtility.CreateAsset<HealSharing>();
    }

    [MenuItem("Assets/Create/Ability/Knockback")]
    public static void CreateBaseKnockbackAsset()
    {
        ScriptableObjectUtility.CreateAsset<Knockback>();
    }

    [MenuItem("Assets/Create/Ability/MaxHPFromConsumable")]
    public static void CreateBaseMaxHPFromConsumableAsset()
    {
        ScriptableObjectUtility.CreateAsset<MaxHPFromConsumable>();
    }

    [MenuItem("Assets/Create/Ability/ReusableItem")]
    public static void CreateBaseReusableItemAsset()
    {
        ScriptableObjectUtility.CreateAsset<ReusableItem>();
    }

    [MenuItem("Assets/Create/Ability/SummonCompanion")]
    public static void CreateBaseSummonCompanionAsset()
    {
        ScriptableObjectUtility.CreateAsset<SummonCompanion>();
    }

        [MenuItem("Assets/Create/Ability/AttackWhenHit")]
    public static void CreateBaseAttackWhenHitAsset()
    {
        ScriptableObjectUtility.CreateAsset<AttackWhenHit>();
    }
}
