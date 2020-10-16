using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelTabUI : PlayerPanelTab
{
    
    public TalentTreeNodeUI prefab;
    public Selectable interactableUp;
    public GameObject talentContainer;
    public TalentTree tree;
    int learned = 0;
    public TalentTreeNodeUI currentNode;
    public ScrollRect talentRect;
    public List<TalentTreeBranchUI> treeBranches;
    public TalentTreeBranchUI branchPrefab;
    public TalentTreeBranchUI currentBranch;

    private void Update()
    {
        if (currentBranch != null)
        {
            talentRect.ScrollRepositionY(currentBranch.GetComponent<RectTransform>(), 25);
        }
        if(player != null && player.talentTree.skillPoints > 0)
        {
                Text text = tabButton.GetComponentInChildren<Text>();
                text.color = Color.yellow;
        } else
        {
            Text text = tabButton.GetComponentInChildren<Text>();
            text.color = Color.black;
        }

    }

    public override void UpdateTab()
    {
        base.UpdateTab();

        if (player != null && player.talentTree.skillPoints > 0)
        {
            Text text = tabButton.GetComponentInChildren<Text>();
            text.color = Color.yellow;
        }
        else
        {
            Text text = tabButton.GetComponentInChildren<Text>();
            text.color = Color.black;
        }
    }

    public void SetTalentTree(Player p)
    {
        player = p;
        tree = player.talentTree;
        foreach(TalentTreeBranchUI branch in treeBranches)
        {
            Destroy(branch.gameObject);
        }
        treeBranches.Clear();



        foreach(TalentTreeBranch branch in tree.branches)
        {
            TalentTreeBranchUI temp;
            temp = Instantiate<TalentTreeBranchUI>(branchPrefab, talentContainer.transform);
            temp.SetBranch(branch, this);
            treeBranches.Add(temp);

        }

        /*
        foreach (Talent talent in tree.talents)
        {

            TalentTreeNodeUI temp;
            temp = Instantiate<TalentTreeNodeUI>(prefab, talentContainer.transform);

            Navigation customNav = temp.GetComponent<Button>().navigation;
            Navigation upNav;

            if (talentNodes.Count == 0)
            {
                customNav.selectOnUp = interactableUp;
                upNav = interactableUp.navigation;
                upNav.selectOnDown = temp.GetComponent<Button>();
                interactableUp.navigation = upNav;
            }
            else
            {
                customNav.selectOnUp = talentNodes[talentNodes.Count-1].GetComponent<Button>();
                upNav = talentNodes[talentNodes.Count - 1].GetComponent<Button>().navigation;
                upNav.selectOnDown = temp.GetComponent<Button>();
                talentNodes[talentNodes.Count - 1].GetComponent<Button>().navigation = upNav;
            }

            temp.GetComponent<Button>().navigation = customNav;

            temp.SetTalent(talent, this);

            talentNodes.Add(temp);
        }

        */
        defaultSelection = treeBranches[0].gameObject;

    }

    public void LearnTalent(string talentName, bool fromLoad = false)
    {
        if(player.talentTree.skillPoints > 0)
        {
            foreach(TalentTreeBranchUI branch in treeBranches)
            {
                foreach (TalentTreeNodeUI talentNode in branch.talentNodes)
                {
                    if (talentNode.talent.talentName.Equals(talentName))
                    {
                        talentNode.OnSelected();
                        return;
                    }
                }
            }

        }

    }


    public void SetCurrentNode(TalentTreeBranchUI branch)
    {
        currentBranch = branch;
        //Debug.Log("Current node is " + node.branchTitle.text);
    }

}
