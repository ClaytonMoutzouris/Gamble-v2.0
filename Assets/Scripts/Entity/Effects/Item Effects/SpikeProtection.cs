using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeProtection : Effect
{
    public SpikeProtection()
    {
        effectName = "Spike Protection";
        type = EffectType.SpikeProtection;
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
