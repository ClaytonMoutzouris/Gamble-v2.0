using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This is a sub-entity, a piece of a larger being
//What does that mean? I guess I'll find out as I work it out.
public class VoidBossHand : EnemyPart
{


    public VoidBossHand(EnemyPrototype proto, Enemy parent) : base(proto, parent)
    {
        Body.mState = ColliderState.Closed;
    }

    public override void EntityUpdate()
    {

        mAttackManager.UpdateAttacks();

        base.EntityUpdate();

    }

}
