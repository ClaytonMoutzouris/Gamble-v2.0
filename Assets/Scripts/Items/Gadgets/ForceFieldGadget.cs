using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldGadget : Gadget
{
    public float duration;
    float elapsedTime = 0;
    ForceField forceField;
    public EntityPrototype forceFieldProto;
    public bool isActive = false;

    public ForceField ForceField { get => forceField; set => forceField = value; }

    public override bool Activate(Player player, int index)
    {
        if (!base.Activate(player, index))
        {
            return false;
        }



        if(isActive)
        {
            return false;
        } else
        {
            isActive = true;
            elapsedTime = 0;
            ForceField = new ForceField(forceFieldProto, player, Vector2.zero, this);
            ForceField.Spawn(player.Position);
        }

        return true;
    }

    public override void OnUnequip(Player player)
    {
        if(ForceField != null)
        {
            ForceField.Die();
        }
        base.OnUnequip(player);
    }

    public override void GadgetUpdate(Player player)
    {
        if(isActive)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= duration)
            {
                ForceField.Die();
                isActive = false;
            }
        } else
        {

        }


        base.GadgetUpdate(player);
    }
}
