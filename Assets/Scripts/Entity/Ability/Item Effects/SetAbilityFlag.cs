using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAbilityFlag : Ability
{
    public AbilityFlag flag;

    public override void OnGainTrigger(Entity entity)
    {
        base.OnGainTrigger(entity);
        entity.abilityFlags.SetFlag(flag, true);
    }

    public override void OnRemoveTrigger(Entity entity)
    {
        base.OnRemoveTrigger(entity);

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
