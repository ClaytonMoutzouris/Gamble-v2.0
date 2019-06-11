using UnityEditor;
using UnityEngine;

public class EnemyPrototypeEditor
{
    [MenuItem("Assets/Create/Enemy Prototype")]
    public static void CreateEnemyPrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<EnemyPrototype>();
    }
}
