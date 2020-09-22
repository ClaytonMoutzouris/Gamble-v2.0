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

    public override void OnUnequipTrigger(Entity entity)
    {
        base.OnUnequipTrigger(entity);

        foreach (Ability ability in entity.abilities)
        {
            if (ability.abilityName.Equals(this.abilityName))
            {
                return;
            }
        }

        entity.abilityFlags.SetFlag(flag, false);


    }
}
