using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    public Dictionary<StatType, Stat> primaryStats;

    public Stats()
    {
        primaryStats = new Dictionary<StatType, Stat>();

        //Create starting stats
        foreach (StatType type in System.Enum.GetValues(typeof(StatType)))
        {
            primaryStats.Add(type, new Stat(type));
        }

    }

    public void SetStats(List<Stat> stats)
    {
        this.primaryStats.Clear();

        foreach (Stat stat in stats)
        {
            this.primaryStats.Add(stat.type, stat);
        }

    }

    public Stat GetStat(StatType type)
    {
        return primaryStats[type];
    }

    public void AddBonus(StatBonus bonus)
    {
        primaryStats[bonus.type].AddBonus(bonus);
    }

    public void RemoveBonus(StatBonus bonus)
    {
        primaryStats[bonus.type].RemoveBonus(bonus);
    }

    public void AddBonuses(List<StatBonus> bonuses)
    {
        foreach(StatBonus bonus in bonuses)
        {
            primaryStats[bonus.type].AddBonus(bonus);
        }

    }

    public void RemoveBonuses(List<StatBonus> bonuses)
    {
        foreach (StatBonus bonus in bonuses)
        {
            primaryStats[bonus.type].RemoveBonus(bonus);
        }

    }
}
//These should only be INTEGERS
public enum StatType { Attack, Defense, Constitution, Speed, Luck }
//These shoudl all be floats
public enum SecondaryStatType { MoveSpeed, BaseHealth, JumpHeight, GravityModifier, GravityAngle, CritChance, CritDamage, AttackSpeed, DodgeChance, ExtraJumps }

public static class StatTypeMethods
{
    public static string GetShortName(StatType type)
    {
        switch (type)
        {
            case StatType.Attack:
                return "ATK";
            case StatType.Defense:
                return "DEF";
            case StatType.Constitution:
                return "CON";
            case StatType.Speed:
                return "SPD";
            case StatType.Luck:
                return "LCK";
        }

        //Should never be reached unless we are trying to cram an enum that doesnt exist
        return "";
    }
}

[System.Serializable]
public class Stat
{
    public StatType type;
    public int value;
    [HideInInspector]
    public List<StatBonus> bonuses;

    public Stat(StatType t, int startingValue = 5)
    {
        type = t;
        value = startingValue;
        bonuses = new List<StatBonus>();
    }

    public void AddBonus(StatBonus bonus)
    {
        bonuses.Add(bonus);
    }

    public void RemoveBonus(StatBonus bonus)
    {
        bonuses.Remove(bonus);
    }

    public int GetBaseValue()
    {
        return value;
    }

    public int GetValue()
    {
        float fullValue = value;
        float multiplier = 0;

        foreach(StatBonus bonus in bonuses)
        {
            switch(bonus.modType)
            {
                case StatModType.Add:
                    fullValue += bonus.bonusValue;
                    break;
                case StatModType.Mult:
                    multiplier += bonus.bonusValue;
                    break;
                default:

                    break;
            }
        }

        return (int)(fullValue + fullValue*multiplier);
    }
}

//The multiplier mod type is really an additive multiplier. Adding a .5 mod adds 50%, it doesn't halve our value. That would be -.5
public enum StatModType { Add, Mult }

[System.Serializable]
public class StatBonus
{
    public StatModType modType = StatModType.Add;
    public StatType type;
    public float bonusValue;

    public StatBonus(StatType t, float min, StatModType modType = StatModType.Add)
    {
        type = t;
        bonusValue = min;
        this.modType = modType;
    }

    public string GetTooltip()
    {
        string tooltip = "";
        if (bonusValue >= 0)
        {
            tooltip += "+";
        }

        switch (modType)
        {
            case StatModType.Add:
                tooltip += bonusValue + " " + type.ToString();
                break;
            case StatModType.Mult:
                tooltip += bonusValue * 100 + "% " + type.ToString();
                break;
        }

        return tooltip;
    }
}