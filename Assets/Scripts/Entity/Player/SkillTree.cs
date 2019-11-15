using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillCategory { Pilot, Sharpshooter, Scientist, Warrior, Diplomat };

public class SkillTree
{
    Dictionary<string, Skill> skills;

    Dictionary<SkillCategory, List<Skill>> skillTrees;
    Player player;

    public SkillTree(Player player)
    {
        this.player = player;
        foreach(SkillCategory category in System.Enum.GetValues(typeof(SkillCategory)))
        {
            //skillTrees.Add
        }
    }

}
