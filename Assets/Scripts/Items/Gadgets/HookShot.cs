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

    public override bool Activate(Player player)
    {
        if (!base.Activate(player))
        {
            return false;
        }

        if(hook != null)
        {
            hook.Die();
        }

        hook = new GrapplingHook(hookProto, player, player.GetAimLeft(), offset, this);


        hook.Spawn(player.Position + offset);
        return true;
    }
}
