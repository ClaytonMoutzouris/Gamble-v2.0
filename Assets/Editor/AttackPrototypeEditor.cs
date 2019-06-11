using UnityEditor;
using UnityEngine;

public class AttackPrototypeEditor
{
    [MenuItem("Assets/Create/Attacks/Melee Attack")]
    public static void CreateMeleeAttackPrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<MeleeAttackPrototype>();
    }

    [MenuItem("Assets/Create/Attacks/Ranged Attack")]
    public static void CreateRangedAttackPrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<RangedAttackPrototype>();
    }
}
