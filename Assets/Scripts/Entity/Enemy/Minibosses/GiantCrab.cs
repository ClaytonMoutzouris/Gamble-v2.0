using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantCrab : Miniboss
{
    public GiantCrab(EnemyPrototype proto) : base(proto)
    {
        Body.mIsKinematic = true;
    }

    public override void EntityUpdate()
    {

        EnemyBehaviour.CheckForTargets(this);


        switch (mEnemyState)
        {
            case EnemyState.Idle:

                break;
            case EnemyState.Moving:


                if (Target != null)
                {
                    Vector2 positionVector = Target.Position - Position;

                    if (positionVector.x > 0)
                    {

                        mDirection = EntityDirection.Right;
                    }
                    else
                    {

                        mDirection = EntityDirection.Left;
                    }

                    if (EnemyBehaviour.TargetInRange(this, Target, 120))
                    {
                        mAttackManager.meleeAttacks[0].Activate();
                    } else
                    {
                        Vector2 dir = (Target.Position - Position).normalized;
                        mAttackManager.rangedAttacks[0].Activate(dir, Position);
                    }


                }



                break;
            case EnemyState.Jumping:

                break;
        }

        base.EntityUpdate();


    }

}
