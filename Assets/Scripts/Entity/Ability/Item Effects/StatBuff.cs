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
        entity.mStats.AddBonuses(statBonuses);

    }

    public override void OnRemoveTrigger(Entity entity)
    {
        base.OnRemoveTrigger(entity);

        entity.mStats.RemoveBonuses(statBonuses);

    }
}
