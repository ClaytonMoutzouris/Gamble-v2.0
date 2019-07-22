using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public Dictionary<StatType, Stat> stats;
    public UIStatContainer uiStats;
    private Entity entity;

    public Stats(Entity e, UIStatContainer ui = null)
    {
        entity = e;
        uiStats = ui;

        stats = new Dictionary<StatType, Stat>();

        //Create starting stats
        foreach (StatType type in System.Enum.GetValues(typeof(StatType)))
        {
            stats.Add(type, new Stat(type));
        }

        //Update the UI
        if(uiStats != null)
        {
            RefreshUI();
        }

    }

    public void RefreshUI()
    {
        //Update the UI
        if (uiStats != null)
        {
            foreach (Stat stat in stats.Values)
            {
                uiStats.SetStat(stat);
            }
        }
    }

    public Stat getStat(StatType type)
    {
        return stats[type];
    }

    public void AddBonuses(List<StatBonus> bonuses)
    {
        foreach(StatBonus bonus in bonuses)
        {
            stats[bonus.type].AddBonus(bonus);
        }

        RefreshUI();
    }

    public void RemoveBonuses(List<StatBonus> bonuses)
    {
        foreach (StatBonus bonus in bonuses)
        {
            stats[bonus.type].RemoveBonus(bonus);
        }

        RefreshUI();
    }
}

public enum StatType { Attack, Defense, Constitution, Speed, Luck };

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

}