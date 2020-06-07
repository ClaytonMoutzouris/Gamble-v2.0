using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TalentTree : ScriptableObject
{
    public string name;
    //public List<TalentTreeBranch> branches;

    public List<Talent> talents;
    public int skillPoints = 0;
}
