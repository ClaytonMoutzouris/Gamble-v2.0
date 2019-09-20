using UnityEditor;
using UnityEngine;

public class ProjectilePrototypeEditor
{
    [MenuItem("Assets/Create/Projectile")]
    public static void CreateProjectileAsset()
    {
        ScriptableObjectUtility.CreateAsset<ProjectilePrototype>();
    }

    [MenuItem("Assets/Create/MeleeAttackObject")]
    public static void CreateMeleeAttackObjectPrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<MeleeAttackObjectPrototype>();
    }
}