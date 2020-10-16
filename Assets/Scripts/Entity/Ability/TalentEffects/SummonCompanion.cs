using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonCompanion : Ability
{

    public CompanionPrototype companionProto;
    public Companion companion;

    public override void OnGainTrigger(Entity entity)
    {
        base.OnGainTrigger(entity);
        Debug.Log("On Equip Trigger");
        companion = new Companion(companionProto);
        if (entity is Player player)
        {
            companion.SetOwner(player);
            companion.Spawn(player.Position);
        }
    }

    public override void OnRemoveTrigger(Entity entity)
    {
        base.OnRemoveTrigger(entity);

        if(entity is Player player)
        {
            player.companionManager.RemoveCompanion(companion);
            if (companion != null)
            {
                companion.Die();
            }
        }

    }

}
