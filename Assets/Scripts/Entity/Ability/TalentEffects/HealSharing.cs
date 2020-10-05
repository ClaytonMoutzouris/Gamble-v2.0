using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSharing : Ability
{

    //Perhaps add a range aspect to this
    public int sharePercent = 10;

    public override void OnHealTrigger(IHurtable player, int heals)
    {
        base.OnHealTrigger(player, heals);
        foreach(Player ally in CrewManager.instance.players)
        {
            if(ally == null)
            {
                continue;
            }
            if(ally != player)
            {
                ally.GainLife(heals*(sharePercent/100), true);
            }
        }

    }
}
