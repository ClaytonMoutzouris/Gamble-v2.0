using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class could probably be generic to handle any kind of stat boost, but for now it will remain specific
public class StatBuff : Ability
{
    public List<StatBonus> statBonuses;

    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);

        player.mStats.AddBonuses(statBonuses);

    }

    public override void OnUnequipTrigger(Player player)
    {
        base.OnUnequipTrigger(player);
        player.mStats.RemoveBonuses(statBonuses);

    }
}
