using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeProtection : Ability
{
    public SpikeProtection()
    {
        abilityName = "Spike Protection";
        type = AbilityType.SpikeProtection;
    }

    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);
        player.spikeProtection = true;
    }

    public override void OnUnequipTrigger(Player player)
    {
        base.OnUnequipTrigger(player);
        player.spikeProtection = false;

    }
}
