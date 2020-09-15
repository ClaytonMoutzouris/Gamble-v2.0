using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonCompanion : Ability
{

    public Vector2 offset = new Vector2(4, 10);
    public CompanionPrototype companionProto;
    public Companion companion;

    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);

        companion = new Companion(companionProto);
        companion.owner = player;
        companion.Spawn(player.Position + offset);
        player.companion = companion;
    }

    public override void OnUnequipTrigger(Player player)
    {
        base.OnUnequipTrigger(player);
        player.companion = null;
        companion.Die();
    }

}
