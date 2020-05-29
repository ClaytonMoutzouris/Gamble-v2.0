using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gadget : Equipment
{
    public float cooldown = 3;
    float cooldownTimer = 0;

    public override void OnEquip(Player player)
    {
        base.OnEquip(player);
    }

    public override void OnUnequip(Player player)
    {
        base.OnUnequip(player);
    }

    public virtual bool Activate(Player player, int index)
    {
        if(cooldownTimer > 0)
        {
            return false;
        }

        cooldownTimer = cooldown;

        return true;
    }

    public virtual void GadgetUpdate(Player player)
    {

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public override string getTooltip()
    {
        string tooltip = base.getTooltip();

        return tooltip;
    }


}
