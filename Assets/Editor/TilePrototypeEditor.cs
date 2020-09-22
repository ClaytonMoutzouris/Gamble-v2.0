using UnityEditor;
using UnityEngine;

public class TilePrototypeEditor
{
    [MenuItem("Assets/Create/TilePrototype")]
    public static void CreateBaseTileAsset()
    {
        ScriptableObjectUtility.CreateAsset<TilePrototype>();
    }
}
