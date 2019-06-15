using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stats
{
    public Dictionary<StatType, Stat> stats;
    public UIStatContainer uiStats;
    public Entity entity;

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
        if (ui != null)
        {
            foreach (Stat stat in stats.Values)
            {
                uiStats.SetStat(stat);
            }
        }


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

    public Stat(StatType t, int startingValue = 5)
    {
        type = t;
        value = startingValue;
    }

}