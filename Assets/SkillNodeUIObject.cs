using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillNodeUIObject : MonoBehaviour
{
    public Image icon;
    public SkillNode node;

    public void SetSkill(SkillNode s)
    {
        node = s;
    }

    public Skill GetSkill()
    {
        return node.skill;
    }

    public void SelectSkill()
    {
        node.SelectNode();
    }
}
