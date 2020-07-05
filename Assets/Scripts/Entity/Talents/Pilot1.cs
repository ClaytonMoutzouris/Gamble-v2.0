using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pilot1 : Talent
{


    public Pilot1()
    {
        name = "Flight Training";
        description = "Adds an extra jump.";
    }

    public override void OnLearned(Player player)
    {
        base.OnLearned(player);

        player.abilities.Add(Ability.GetEffectFromType(AbilityType.ExtraJump));

    }
}
