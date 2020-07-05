using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Maybe sets the new standard for effects being generic
public class ExtraDamage : Ability
{
    StatBonus statBonus;
    public ExtraDamage()
    {
        abilityName = "Extra Damage";
        type = AbilityType.ExtraDamage;
        statBonus = new StatBonus(StatType.Attack, 5);
    }

    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);

        player.mStats.AddBonus(statBonus);
    }

    public override void OnUnequipTrigger(Player player)
    {
        base.OnUnequipTrigger(player);
        player.mStats.RemoveBonus(statBonus);

    }

}
