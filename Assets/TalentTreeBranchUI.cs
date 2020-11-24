using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TalentTreeBranchUI : MonoBehaviour, ISelectHandler
{
    TalentTreeBranch treeBranch;
    public TalentTreeNodeUI prefab;
    public List<TalentTreeNodeUI> talentNodes;
    public PlayerLevelTabUI talentTree;
    public Button interactable;

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
        SetNavigation();
    }

    public void SetNavigation()
    {
        Debug.Log("Talent Nodes count " + talentNodes.Count);
        for (int i = 0; i < talentNodes.Count; i++)  
        {

            Navigation customNav = talentNodes[i].GetComponent<Button>().navigation;
            Navigation iNav = interactable.navigation;

            if (i == 0)
            {
                iNav.selectOnRight = talentNodes[i].GetComponent<Button>();

                customNav.selectOnLeft = talentNodes[talentNodes.Count-1].GetComponent<Button>();
                if (talentNodes.Count > 1)
                {
                    customNav.selectOnRight = talentNodes[i + 1].GetComponent<Button>();
                }

            }
            else if (i == talentNodes.Count - 1)
            {
                customNav.selectOnRight = talentNodes[0].GetComponent<Button>();
                if (talentNodes.Count > 1)
                {
                    customNav.selectOnLeft = talentNodes[i - 1].GetComponent<Button>();
                }

            }
            else
            {
                customNav.selectOnLeft = talentNodes[i - 1].GetComponent<Button>();
                customNav.selectOnRight = talentNodes[i + 1].GetComponent<Button>();
            }

            customNav.selectOnUp = interactable;
            customNav.selectOnDown = interactable;

            interactable.navigation = iNav;
            talentNodes[i].GetComponent<Button>().navigation = customNav;

        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        talentTree.SetCurrentNode(this);
        talentTree.panel.tooltip.SetTooptip(GetTooltip());

    }

    public void SelectBranch()
    {
        GamepadInputManager.instance.gamepadInputs[talentTree.panel.playerIndex].GetEventSystem().SetSelectedGameObject(talentNodes[0].gameObject);

    }

    public bool LearnTalent(int index)
    {
        return treeBranch.LearnTalentAtIndex(index);
    }

    public string GetTooltip()
    {
        return treeBranch.branchName;
    }

}
