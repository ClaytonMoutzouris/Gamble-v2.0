using UnityEditor;
using UnityEngine;

public class TilePrototypeEditor
{
    [MenuItem("Assets/Create/Tile")]
    public static void CreateBaseTileAsset()
    {
        ScriptableObjectUtility.CreateAsset<TilePrototype>();
    }
}
