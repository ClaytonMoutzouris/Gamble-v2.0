using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TalentTreeNodeUI : MonoBehaviour, ISelectHandler
{
    PlayerLevelTabUI talentTree;
    public Talent talent;
    public Image background;
    public Image branchIcon;
    public Text branchTitle;

    public void Start()
    {
        //talentTree = GetComponentInParent<PlayerLevelTabUI>();
    }

    public void SetTalent(Talent t, PlayerLevelTabUI tree)
    {
        talentTree = tree;
        talent = t;
        branchTitle.text = talent.talentName + "\n" + talent.description;
        branchIcon.sprite = talent.icon;
    }

    public void SetColor()
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
        talentTree.LearnTalent(talent.talentName);
        if(talent.isLearned)
        {
            SetColor();
        }
    }

    public string GetTooltip()
    {
        return talent.description;
    }

    public void OnSelect(BaseEventData eventData)
    {
        talentTree.SetCurrentNode(this);
    }
}
