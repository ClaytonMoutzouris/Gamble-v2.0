using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : ConsumableItem
{
    public int value = 20;

    public override void Use(Player player, int index)
    {
        player.Health.GainHP(value);

        base.Use(player, index);
    }
}
