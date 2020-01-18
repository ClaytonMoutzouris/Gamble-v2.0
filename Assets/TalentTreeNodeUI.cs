using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentTreeNodeUI : MonoBehaviour
{
    TalentTreeBranch treeBranch;
    public Image branchIcon;
    public Text branchTitle;

    public void SetBranch(TalentTreeBranch branch)
    {
        treeBranch = branch;
        //branchTitle.text = treeBranch.name;
    }

    public void SelectBranch()
    {
        //treeBranch.LearnNextNode();
    }

    public string GetTooltip()
    {
        return treeBranch.talents[treeBranch.GetLearned()].description;
    }
}
