using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Slime : Enemy {

    public Slime(EnemyPrototype proto) : base(proto)
    {
        //Body.mAABB.ScaleX *= -1;

        Body.mIsKinematic = false;
        //Behaviour
        closeRange = 30f;
        midRange = 70f;
        longRange = 140f;
    }

    public override void EntityUpdate()
    {
        switch (mEnemyState)
        {
            case EnemyState.Idle:
                Renderer.SetAnimState("Idle");
                break;
            case EnemyState.Moving:
                Renderer.SetAnimState("Slime_Move");
                EnemyBehaviour.MoveHostile(this);
                break;
            case EnemyState.Jumping:
                break;
            case EnemyState.Attacking:

                bool done = true;

                foreach(Attack attack in mAttackManager.meleeAttacks)
                {
                    if (attack.mIsActive)
                    {
                        Renderer.SetAnimState("Slime_Attack");
                        done = false;
                    }
                }

                if (done)
                {
                    mEnemyState = EnemyState.Moving;
                }
                break;
        }

        base.EntityUpdate();
    }

}
