using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectItem : ConsumableItem
{
    public StatusEffect effect;

    public override bool Use(Player player)
    {
        StatusEffect temp = ScriptableObject.Instantiate(effect);
        temp.OnApplyEffect(player);
        return base.Use(player);
    }

    public override string GetTooltip()
    {
        string tooltip = base.GetTooltip();
        tooltip += "\nApplies " + effect.ToString() + " when used.";

        return tooltip;

    }
}
