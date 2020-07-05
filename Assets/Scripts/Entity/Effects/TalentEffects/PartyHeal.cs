using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyHeal : Ability
{

    public PartyHeal()
    {
        abilityName = "Party Heal";
        type = AbilityType.PartyHeal;
    }

    public override void OnHealTrigger(Player player, int heals)
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
                ally.GainLife(heals, true);
            }
        }

    }
}
