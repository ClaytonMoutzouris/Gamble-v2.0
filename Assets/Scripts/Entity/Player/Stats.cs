﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    public Dictionary<StatType, Stat> stats;
    private Entity entity;

    public Stats(Entity e)
    {
        entity = e;

        stats = new Dictionary<StatType, Stat>();

        //Create starting stats
        foreach (StatType type in System.Enum.GetValues(typeof(StatType)))
        {
            stats.Add(type, new Stat(type));
        }

    }

    public void SetStats(List<Stat> stats)
    {
        this.stats.Clear();

        foreach (Stat stat in stats)
        {
            this.stats.Add(stat.type, stat);
        }

    }

    public Stat GetStat(StatType type)
    {
        return stats[type];
    }

    public void AddBonus(StatBonus bonus)
    {
        stats[bonus.type].AddBonus(bonus);
    }

    public void RemoveBonus(StatBonus bonus)
    {
        stats[bonus.type].RemoveBonus(bonus);
    }

    public void AddBonuses(List<StatBonus> bonuses)
    {
        foreach(StatBonus bonus in bonuses)
        {
            stats[bonus.type].AddBonus(bonus);
        }

    }

    public void RemoveBonuses(List<StatBonus> bonuses)
    {
        foreach (StatBonus bonus in bonuses)
        {
            stats[bonus.type].RemoveBonus(bonus);
        }

    }
}

public enum StatType { Attack, Defense, Constitution, Speed, Luck };
public enum AdvancedStat { MoveSpeed, BaseHealth, JumpHeight };

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
        Debug.Log(bonuses.Remove(bonus).ToString());
    }

    public int GetBaseValue()
    {
        return value;
    }

    public int GetValue()
    {
        int fullValue = value;

        foreach(StatBonus bonus in bonuses)
        {
            fullValue += bonus.bonusValue;
        }

        return fullValue;
    }
}

[System.Serializable]
public class StatBonus
{
    public StatType type;
    public int bonusValue;

    public StatBonus(StatType t, int min)
    {
        type = t;
        bonusValue = min;
    }

    public string getTooltip()
    {
        string tooltip = "";

        tooltip += type.ToString() + " +" + bonusValue; 

        return tooltip;
    }
}