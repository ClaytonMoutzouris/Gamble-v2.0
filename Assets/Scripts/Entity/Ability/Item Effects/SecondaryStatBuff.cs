using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class could probably be generic to handle any kind of stat boost, but for now it will remain specific
public class SecondaryStatBuff : Ability
{
    public List<SecondaryStatBonus> statBonuses;

    public override void OnGainTrigger(Entity entity)
    {
        base.OnGainTrigger(entity);
        entity.mStats.AddSecondaryBonuses(statBonuses);

    }

    public override void OnRemoveTrigger(Entity entity)
    {
        base.OnRemoveTrigger(entity);

        entity.mStats.RemoveSecondaryBonuses(statBonuses);

    }
}
