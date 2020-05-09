using UnityEditor;
using UnityEngine;

public class WorldEditor
{

    [MenuItem("Assets/Create/World")]
    public static void CreateWorldAsset()
    {
        ScriptableObjectUtility.CreateAsset<WorldData>();
    }
}
