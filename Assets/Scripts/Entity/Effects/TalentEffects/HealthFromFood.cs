using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHPFromFood : Effect
{


    public MaxHPFromFood()
    {
        effectName = "Max HP From Food";
        type = EffectType.MaxHPFromFood;
    }

    public override void OnConsumeItem(Player player, ConsumableItem item)
    {
        base.OnConsumeItem(player, item);

        if(item is Food)
        {
            player.Health.baseHP += 1;
            player.Health.UpdateHealth();
            player.GainLife(1);
        }

    }
}
