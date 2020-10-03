using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonCompanion : Ability
{

    public CompanionPrototype companionProto;
    public Companion companion;

    public override void OnEquipTrigger(Entity entity)
    {
        base.OnEquipTrigger(entity);
        Debug.Log("On Equip Trigger");
        companion = new Companion(companionProto);
        if (entity is Player player)
        {
            companion.SetOwner(player);
            companion.Spawn(player.Position);
        }
    }

    public override void OnUnequipTrigger(Entity entity)
    {
        base.OnUnequipTrigger(entity);

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
