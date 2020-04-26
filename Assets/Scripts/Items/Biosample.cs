using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biosample : Item
{

    public WorldType sampleWorldType;

    public override List<InventoryOption> GetInventoryOptions()
    {
        return new List<InventoryOption>() {
            InventoryOption.Move,
            InventoryOption.Drop,
            InventoryOption.Cancel };
    }

    public override bool Identify()
    {
        //This is bad practice, but works for now
        if (base.Identify())
        {
            if(LevelManager.instance.NumCompletedWorlds() < 5)
            {
                sampleWorldType = (WorldType)Random.Range(0, (int)WorldType.Void);
            }
             else if(LevelManager.instance.NumCompletedWorlds() > 5)
            {
                sampleWorldType = (WorldType)Random.Range(0, (int)WorldType.Count);
            }
            else
            {
                sampleWorldType = WorldType.Void;
            }
            return true;
        }

        return false;
    }

    public override string getTooltip()
    {
        string tooltip = "";

        if (identified)
        {
            tooltip += "\n<color=white>" + sampleWorldType + " Biosample</color>";
        }
        else
        {
            tooltip += "<color=white>Unknown Biosample</color>";
        }


        return tooltip;
    }
}