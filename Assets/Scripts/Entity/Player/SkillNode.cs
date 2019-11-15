using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillNode
{

    public bool owned = false;
    public Skill skill;
    public Skill prerequisite = null;

    public SkillNode(Skill skill, Skill prereq = null)
    {
        this.skill = skill;
        prerequisite = prereq;
    }

    public void SelectNode()
    {
        owned = true;
    }


}
