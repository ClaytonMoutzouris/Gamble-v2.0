using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsFromFood : Effect
{


    public StatsFromFood()
    {
        effectName = "Stats From Food";
        type = EffectType.StatsFromFood;
    }

    public override void OnConsumeItem(Player player, ConsumableItem item)
    {
        base.OnConsumeItem(player, item);

        if(item is Food)
        {
            player.mStats.AddBonus(new StatBonus((StatType)Random.Range(0, (int)StatType.Luck + 1), 1));
        }

    }
}
