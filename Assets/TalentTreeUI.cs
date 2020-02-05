using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentTreeUI : MonoBehaviour
{
    
    public TalentTreeNodeUI prefab;
    public List<TalentTreeNodeUI> talentNodes;
    public Selectable interactableUp;

    Player player;
    public TalentTree tree;
    int learned = 0;

    public void SetTalentTree(Player p)
    {
        player = p;
        tree = player.talentTree;



        foreach (Talent talent in tree.talents)
        {



            TalentTreeNodeUI temp;
            temp = Instantiate<TalentTreeNodeUI>(prefab, transform);

            Navigation customNav = temp.GetComponent<Button>().navigation;
            Navigation leftNav, upNav;

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

            temp.SetTalent(talent);

            talentNodes.Add(temp);
        }
    }

    public void LearnNextNode()
    {

        tree.talents[learned].OnLearned(player);
        learned++;
    }

    public void LearnNodeAtIndex(int index)
    {
        tree.talents[index].OnLearned(player);
        talentNodes[index].OnLearned();
    }
}
