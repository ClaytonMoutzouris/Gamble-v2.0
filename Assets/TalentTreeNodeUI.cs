using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentTreeNodeUI : MonoBehaviour
{
    TalentTreeUI talentTree;
    Talent talent;
    public Image background;
    public Image branchIcon;
    public Text branchTitle;
    public bool learned = false;

    public void Start()
    {
        talentTree = GetComponentInParent<TalentTreeUI>();
    }

    public void SetTalent(Talent t)
    {
        talent = t;
        branchTitle.text = talent.name + "\n" + talent.description;
        branchIcon.sprite = talent.icon;
    }

    public void OnLearned()
    {
        learned = true;
        background.color = Color.yellow;
    }

    public void OnSelected()
    {
        if (learned)
            return;
        talentTree.LearnNodeAtIndex(transform.GetSiblingIndex());
    }

    public string GetTooltip()
    {
        return talent.description;
    }


}
