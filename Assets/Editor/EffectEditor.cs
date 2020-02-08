using UnityEditor;
using UnityEngine;

public class EffectEditor
{


    [MenuItem("Assets/Create/Effects/EntityEffects/SpawnOnDeath")]
    public static void CreateBaseSpawnOnDeathAsset()
    {
        ScriptableObjectUtility.CreateAsset<Spawn>();
    }


}