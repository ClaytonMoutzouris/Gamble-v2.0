using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoUI : PlayerPanelTab
{
    public UIStatContainer statContainer;
    public UIEffectContainer effectContainer;

    public override void Open()
    {
        base.Open();

        Debug.Log("THIS IS GETTING CALLED");

        if (player != null)
        {
            Debug.Log("PLAYER NOT NULL");

            effectContainer.ClearEffects();
            foreach(Ability ability in player.abilities)
            {
                effectContainer.AddEffect(ability);
                Debug.Log("ADDING ABILITY " + ability.abilityName);

            }

            foreach (StatType stat in player.mStats.stats.Keys)
            {
                statContainer.SetStat(player.mStats.GetStat(stat));
            }
        }


    }
}
