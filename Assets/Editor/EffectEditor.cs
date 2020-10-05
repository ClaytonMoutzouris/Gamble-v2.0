using UnityEditor;
using UnityEngine;

public class EffectEditor
{

    [MenuItem("Assets/Create/StatusEffects/DamageOverTimeEffect")]
    public static void CreateBaseDamageOverTimeEffectAsset()
    {
        ScriptableObjectUtility.CreateAsset<DamageOverTimeEffect>();
    }

    [MenuItem("Assets/Create/StatusEffects/StunEffect")]
    public static void CreateBaseStunEffectAsset()
    {
        ScriptableObjectUtility.CreateAsset<StunEffect>();
    }

    [MenuItem("Assets/Create/StatusEffects/GravityEffect")]
    public static void CreateBaseGravityEffectAsset()
    {
        ScriptableObjectUtility.CreateAsset<GravityShiftEffect>();
    }

    [MenuItem("Assets/Create/StatusEffects/HealOverTimeEffect")]
    public static void CreateBaseHealOverTimeEffectAsset()
    {
        ScriptableObjectUtility.CreateAsset<HealOverTimeEffect>();
    }

    [MenuItem("Assets/Create/StatusEffects/AbilityEffect")]
    public static void CreateBaseAbilityEffectAsset()
    {
        ScriptableObjectUtility.CreateAsset<AbilityEffect>();
    }

}
