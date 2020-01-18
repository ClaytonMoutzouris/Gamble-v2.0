using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard1 : Talent
{
    public Guard1()
    {
        name = "Tough Guy";
        description = "Gain 2 DEF and 2 CON.";
    }

    public override void OnLearned(Player player)
    {
        base.OnLearned(player);
        player.mStats.AddBonus(new StatBonus(StatType.Defense, 2));
        player.mStats.AddBonus(new StatBonus(StatType.Constitution, 2));


    }
}
