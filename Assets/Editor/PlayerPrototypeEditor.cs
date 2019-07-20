using UnityEditor;
using UnityEngine;

public class PlayerPrototypeEditor
{
    [MenuItem("Assets/Create/Player Prototype")]
    public static void CreateEnemyPrototypeAsset()
    {
        ScriptableObjectUtility.CreateAsset<PlayerPrototype>();
    }
}
