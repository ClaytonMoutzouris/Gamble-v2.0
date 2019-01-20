using UnityEditor;
using UnityEngine;

public class MapDataEditor
{

    [MenuItem("Assets/Create/Map")]
    public static void CreateMapAsset()
    {
        ScriptableObjectUtility.CreateAsset<MapData>();
    }
}
