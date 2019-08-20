using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : ConsumableItem
{
    public int value = 50;

    public override void Use(Player player, int index)
    {
        player.Health.GainHP(value);

        base.Use(player, index);
    }

    public override string getTooltip()
    {
        string tooltip = base.getTooltip();
        tooltip += "\nRestores " + value.ToString() + " health when eaten.";

        return tooltip;

    }
}
