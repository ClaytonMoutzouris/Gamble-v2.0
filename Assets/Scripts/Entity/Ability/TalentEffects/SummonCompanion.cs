using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonCompanion : Ability
{

    public CompanionPrototype companionProto;
    public Companion companion;

    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);

        companion = new Companion(companionProto);
        companion.SetOwner(player);
        companion.Spawn(player.Position);
    }

    public override void OnUnequipTrigger(Entity entity)
    {
        base.OnUnequipTrigger(entity);

        if(entity is Player player)
        {
            player.companionManager.RemoveCompanion(companion);
            companion.Die();
        }

    }

}
