using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAbilityFlag : Ability
{
    public AbilityFlag flag;

    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);
        player.abilityFlags.SetFlag(flag, true);
    }

    public override void OnUnequipTrigger(Player player)
    {
        base.OnUnequipTrigger(player);

        foreach (Ability ability in player.abilities)
        {
            if (ability.abilityName.Equals(this.abilityName))
            {
                return;
            }
        }

        player.abilityFlags.SetFlag(flag, false);


    }
}
