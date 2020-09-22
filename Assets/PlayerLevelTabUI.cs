using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelTabUI : PlayerPanelTab
{
    
    public TalentTreeNodeUI prefab;
    public List<TalentTreeNodeUI> talentNodes;
    public Selectable interactableUp;
    public GameObject talentContainer;
    public TalentTree tree;
    int learned = 0;
    public TalentTreeNodeUI currentNode;
    public ScrollRect talentRect;

    private void Update()
    {
        if (currentNode != null)
        {
            talentRect.ScrollRepositionY(currentNode.GetComponent<RectTransform>(), 25);
        }
    }

    public void SetTalentTree(Player p)
    {
        player = p;
        tree = player.talentTree;
        foreach(TalentTreeNodeUI node in talentNodes)
        {
            Destroy(node.gameObject);
        }
        talentNodes.Clear();

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

        defaultSelection = talentNodes[0].gameObject;
    }

    public void LearnNodeAtIndex(int index)
    {
        if (player.talentTree.skillPoints > 0)
        {
            tree.talents[index].OnLearned(player);
            player.talentTree.skillPoints--;
        }
    }

    public void LearnTalent(string talentName, bool fromLoad = false)
    {
        if(player.talentTree.skillPoints > 0)
        {
            foreach (TalentTreeNodeUI talentNode in talentNodes)
            {
                if (talentNode.talent.talentName.Equals(talentName) && (!talentNode.talent.isLearned || talentNode.talent.repeatable))
                {
                    Debug.Log("Found Talent " + talentName);
                    talentNode.talent.OnLearned(player, fromLoad);
                    player.talentTree.skillPoints--;
                    talentNode.SetColor();
                    return;
                }
            }
        }

    }

    public void SetCurrentNode(TalentTreeNodeUI node)
    {
        currentNode = node;
        //Debug.Log("Current node is " + node.branchTitle.text);
    }
}
