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

    public void GetNewTree()
    {
        List<Talent> temp = new List<Talent>();

        foreach(Talent talent in talents)
        {
            temp.Add(Instantiate(talent));
        }

        talents = temp;
    }
}
