using UnityEditor;
using UnityEngine;

public class ProjectilePrototypeEditor
{
    [MenuItem("Assets/Create/Projectile")]
    public static void CreateMeleeAttackPrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<ProjectilePrototype>();
    }
}