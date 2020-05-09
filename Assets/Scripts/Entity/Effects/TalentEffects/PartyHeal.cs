using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyHeal : Effect
{

    public PartyHeal()
    {
        effectName = "Party Heal";
        type = EffectType.PartyHeal;
    }

    public override void OnHealTrigger(Player player, int heals)
    {
        base.OnHealTrigger(player, heals);
        foreach(Player ally in GameManager.instance.players)
        {
            if(ally == null)
            {
                continue;
            }
            if(ally != player)
            {
                ally.GainLife(heals);
            }
        }

    }
}
