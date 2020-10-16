using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class could probably be generic to handle any kind of stat boost, but for now it will remain specific
public class StatBuff : Ability
{
    public List<StatBonus> statBonuses;

    public override void OnGainTrigger(Entity entity)
    {
        base.OnGainTrigger(entity);
        //TODO: Add stats to all entities
        if(entity is Player player)
        {
            player.mStats.AddBonuses(statBonuses);
        }

        if(entity is Enemy enemy)
        {
            enemy.mStats.AddBonuses(statBonuses);

        }

    }

    public override void OnRemoveTrigger(Entity entity)
    {
        base.OnRemoveTrigger(entity);
        if(entity is Player player)
        {
            player.mStats.RemoveBonuses(statBonuses);
        } else if(entity is Enemy enemy)
        {
            enemy.mStats.RemoveBonuses(statBonuses);
        }

    }
}
