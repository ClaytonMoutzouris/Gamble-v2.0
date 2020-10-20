using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TalentTreeNodeUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Talent talent;
    public Image branchIcon;
    public TalentTreeBranchUI branchParent;

    public void Start()
    {
        //talentTree = GetComponentInParent<PlayerLevelTabUI>();
    }

    public void SetTalent(Talent t, TalentTreeBranchUI parent)
    {
        branchParent = parent;
        talent = t;
        //branchTitle.text = talent.talentName + "\n" + talent.description;
        branchIcon.sprite = talent.icon;
    }

    public void SetColor()
    {
        if(talent.repeatable)
        {
            branchIcon.color = Color.green;
        }
        else
        {
            branchIcon.color = Color.yellow;
        }
    }

    public void OnSelected()
    {
        if(branchParent.LearnTalent(transform.GetSiblingIndex()))
        {
            SetColor();
        }
    }

    public string GetTooltip()
    {
        string tooltip = talent.talentName + "\n" + talent.description;

        foreach(Ability ability in talent.abilities)
        {
            tooltip += ability.GetDescription();
        }
        return tooltip;
    }

    public void OnSelect(BaseEventData eventData)
    {
        branchParent.talentTree.panel.tooltip.SetTooptip(GetTooltip());
    }

    public void OnDeselect(BaseEventData eventData)
    {
        branchParent.talentTree.panel.tooltip.ClearTooltip();
    }
}
