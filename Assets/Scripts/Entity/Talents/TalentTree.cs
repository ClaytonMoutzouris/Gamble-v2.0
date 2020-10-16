using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TalentTree : ScriptableObject
{
    public string name;
    public List<TalentTreeBranch> branches;

    public int skillPoints = 0;
    public Player player;

    public void GetNewTree(Player player)
    {
        this.player = player;
        foreach(TalentTreeBranch branch in branches)
        {
            branch.GetNewBranch(this);
        }

    }

    public List<Talent> GetAllTalents()
    {
        List<Talent> fullList = new List<Talent>();

        foreach (TalentTreeBranch branch in branches)
        {
            fullList.AddRange(branch.talents);
        }

        return fullList;

    }

}
