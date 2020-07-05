using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy : Ability
{
    public Heavy()
    {
        abilityName = "Heavy";
        type = AbilityType.Heavy;
    }

    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);

        player.Body.mIsHeavy = true;
    }

    public override void OnUnequipTrigger(Player player)
    {
        base.OnUnequipTrigger(player);

        //Straight up set this to false because player cant be heavy otherwise yet
        player.Body.mIsHeavy = false;

    }
}
