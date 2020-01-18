using UnityEditor;
using UnityEngine;

public class PlayerEditor
{

    [MenuItem("Assets/Create/Player/Class")]
    public static void CreateBasePlayerClassAsset()
    {
        ScriptableObjectUtility.CreateAsset<PlayerClass>();
    }

    [MenuItem("Assets/Create/Player/Background")]
    public static void CreateBasePlayerBackgroundAsset()
    {
        ScriptableObjectUtility.CreateAsset<PlayerBackground>();
    }

}
