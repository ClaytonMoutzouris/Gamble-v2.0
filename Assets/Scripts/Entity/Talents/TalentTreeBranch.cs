using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This should be a scriptable object
[System.Serializable]
public class TalentTreeBranch
{
    public List<Talent> talents;
    int learned = 0;
    int branchIndex;
    TalentTree talentTree;

    public TalentTreeBranch()
    {
    }

    public void GetNewBranch(TalentTree tree)
    {
        talentTree = tree;
        List<Talent> tempList = new List<Talent>();

        foreach (Talent talent in talents)
        {
            tempList.Add(ScriptableObject.Instantiate(talent));
        }

        talents = tempList;
    }

    public int GetLearned()
    {
        return learned;
    }

    public void LearnNextNode(Player player)
    {
        if(learned >= talents.Count)
        {
            return;
        }

        talents[learned].OnLearned(player);
        learned++;
    }

    public bool LearnTalentAtIndex(int index)
    {
        if(index > learned || (talents[index].isLearned && !talents[index].repeatable) || talentTree.skillPoints <= 0)
        {
            return false;
        }
        talents[index].OnLearned(talentTree.player);
        learned++;
        talentTree.skillPoints--;

        return true;
    }


}
