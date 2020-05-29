using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShot : Gadget
{

    //Range of the hookshot in tiles
    public int range = 10;
    public EntityPrototype hookProto;
    public Vector2 offset;
    public GrapplingHook hook;

    public override bool Activate(Player player, int index)
    {
        if (!base.Activate(player, index))
        {
            return false;
        }

        if(hook != null)
        {
                hook.Die();
        }

        Vector2 aim;

        if(index == 1)
        {
            aim = player.GetAimLeft();
        } else
        {
            aim = player.GetAimRight();
        }

        hook = new GrapplingHook(hookProto, player, aim, offset, this);


        hook.Spawn(player.Position + offset);
        return true;
    }
}
