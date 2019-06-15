﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : ConsumableItem
{
    public int value = 20;

    public override void Use(Player player, int index)
    {
        player.mHealth.GainHP(value);

        base.Use(player, index);
    }
}
