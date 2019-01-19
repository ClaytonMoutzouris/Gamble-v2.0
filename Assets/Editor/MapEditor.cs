using UnityEditor;
using UnityEngine;

public class MapEditor
{

    [MenuItem("Assets/Create/Map")]
    public static void CreateMapAsset()
    {
        ScriptableObjectUtility.CreateAsset<MapData>();
    }
}
