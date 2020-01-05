using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class could probably be generic to handle any kind of stat boost, but for now it will remain specific
public class SuperSpeed : Effect
{
    int statBoost = 10;
    StatType effectedStat = StatType.Speed;
    StatBonus speedBonus;

    public SuperSpeed()
    {
        effectName = "Super Speed";
        type = EffectType.SuperSpeed;
        speedBonus = new StatBonus(effectedStat, statBoost);
    }

    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);

        player.mStats.AddBonus(speedBonus);
    }

    public override void OnUnequipTrigger(Player player)
    {
        base.OnUnequipTrigger(player);
        player.mStats.RemoveBonus(speedBonus);

    }
}
