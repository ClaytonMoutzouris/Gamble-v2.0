using UnityEditor;
using UnityEngine;

public class RoomModelEditor {

    [MenuItem("Assets/Create/Room Model")]
    public static void CreateSkillAsset()
    {
        ScriptableObjectUtility.CreateAsset<RoomModel>();
    }
}
