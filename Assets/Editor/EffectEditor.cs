using UnityEditor;
using UnityEngine;

public class EffectEditor
{

    [MenuItem("Assets/Create/Effects/Poisoned")]
    public static void CreateBaseLootTableAsset()
    {
        ScriptableObjectUtility.CreateAsset<Poisoned>();
    }

}
