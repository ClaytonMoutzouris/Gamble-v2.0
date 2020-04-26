using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gadget : Equipment
{


    public override void OnEquip(Player player)
    {
        base.OnEquip(player);
    }

    public override void OnUnequip(Player player)
    {
        base.OnUnequip(player);
    }

    public override string getTooltip()
    {
        string tooltip = base.getTooltip();

        return tooltip;
    }
}
