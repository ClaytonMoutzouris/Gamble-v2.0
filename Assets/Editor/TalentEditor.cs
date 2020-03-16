﻿using UnityEditor;
using UnityEngine;

public class TalentEditor
{

    [MenuItem("Assets/Create/Talents/TalentTree")]
    public static void CreateTalentTreeAsset()
    {
        ScriptableObjectUtility.CreateAsset<TalentTree>();
    }

    [MenuItem("Assets/Create/Talents/Talent")]
    public static void CreateTalentAsset()
    {
        ScriptableObjectUtility.CreateAsset<Talent>();
    }
}