﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : ConsumableItem
{
    public int value = 20;

    public override bool Use(Player player)
    {
        player.GainLife(value);

        return base.Use(player);
    }

    public override string getTooltip()
    {
        string tooltip = base.getTooltip();
        tooltip += "\nRestores " + value.ToString() + " health when eaten.";

        return tooltip;

    }
}
