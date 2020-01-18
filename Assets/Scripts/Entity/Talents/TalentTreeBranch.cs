using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TalentTreeBranch
{
    public Talent[] talents;
    int learned = 0;

    public TalentTreeBranch()
    {
    }

    public int GetLearned()
    {
        return learned;
    }

    public void LearnNextNode(Player player)
    {

        talents[learned].OnLearned(player);
        learned++;
    }


}
