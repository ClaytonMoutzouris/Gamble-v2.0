using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentTree : ScriptableObject
{
    public string name;
    public List<TalentTreeBranch> branches;

    public int skillPoints = 0;
    public Player player;

    public void GetNewTree(Player player)
    {
        List<TalentTreeBranch> newBranches = new List<TalentTreeBranch>();
        this.player = player;
        foreach (TalentTreeBranch branch in branches)
        {
            TalentTreeBranch temp = Instantiate(branch);
            temp.GetNewBranch(this);
            newBranches.Add(temp);

        }

        branches = newBranches;
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
