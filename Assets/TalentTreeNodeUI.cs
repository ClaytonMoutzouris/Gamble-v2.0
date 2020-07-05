using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentTreeNodeUI : MonoBehaviour
{
    PlayerLevelTabUI talentTree;
    Talent talent;
    public Image background;
    public Image branchIcon;
    public Text branchTitle;

    public void Start()
    {
        talentTree = GetComponentInParent<PlayerLevelTabUI>();
    }

    public void SetTalent(Talent t)
    {
        talent = t;
        branchTitle.text = talent.name + "\n" + talent.description;
        branchIcon.sprite = talent.icon;
    }

    public void OnLearned()
    {
        if(talent.repeatable)
        {
            background.color = Color.green;
        }
        else
        {
            background.color = Color.yellow;
        }
    }

    public void OnSelected()
    {
        if (talent.isLearned && !talent.repeatable)
            return;

        talentTree.LearnNodeAtIndex(transform.GetSiblingIndex());
    }

    public string GetTooltip()
    {
        return talent.description;
    }


}
