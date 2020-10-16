using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TalentTreeBranchUI : MonoBehaviour, ISelectHandler
{
    TalentTreeBranch treeBranch;
    public TalentTreeNodeUI prefab;
    public List<TalentTreeNodeUI> talentNodes;
    public PlayerLevelTabUI talentTree;

    public void SetBranch(TalentTreeBranch branch, PlayerLevelTabUI tree)
    {
        talentTree = tree;
        treeBranch = branch;
        foreach (Talent talent in treeBranch.talents)
        {
            TalentTreeNodeUI temp;
            temp = Instantiate<TalentTreeNodeUI>(prefab, transform);
            temp.SetTalent(talent, this);
            talentNodes.Add(temp);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        talentTree.SetCurrentNode(this);
    }

    public void SelectBranch()
    {
        GamepadInputManager.instance.gamepadInputs[talentTree.panel.playerIndex].GetEventSystem().SetSelectedGameObject(talentNodes[0].gameObject);

    }

    public bool LearnTalent(int index)
    {
        return treeBranch.LearnTalentAtIndex(index);
    }

}
