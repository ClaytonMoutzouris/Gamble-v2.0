using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Drone : Companion
{


    public Drone(CompanionPrototype proto) : base(proto)
    {



    }

    public override void EntityUpdate()
    {

        Position = Vector2.Lerp(Position, owner.Position + new Vector2(32, 32), mMovingSpeed / 10);

        if (owner.AttackManager.rangedAttacks[0].mIsActive)
        {

            mAttackManager.rangedAttacks[0].Activate(owner.GetAimRight(), Position);

        }


        base.EntityUpdate();





    }

    public override void SetOwner(Player player)
    {
        base.SetOwner(player);
        Debug.Log("Setting drones proto");
        mAttackManager.rangedAttacks[0] = new RangedAttack(this, (RangedAttackPrototype)owner.AttackManager.rangedAttacks[0].attackPrototype);
    }

}
