using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHPFromConsumable : Ability
{

    public int maxHpBonus;

    public override void OnConsumeItem(Player player, ConsumableItem item)
    {
        base.OnConsumeItem(player, item);

        if(item is Food)
        {
            player.Health.baseHP += maxHpBonus;
            player.Health.UpdateHealth();
            player.GainLife(maxHpBonus);
        }

    }
}
