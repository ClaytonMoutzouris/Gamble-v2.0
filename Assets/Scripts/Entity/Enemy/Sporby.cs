using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sporby : Enemy
{

    public Sporby(EnemyPrototype proto) : base(proto)
    {

    }

    public override void EntityUpdate()
    {

        EnemyBehaviour.CheckForTargets(this);

        switch (mEnemyState)
        {
            case EnemyState.Idle:

                break;
            case EnemyState.Moving:

                if (Body.mPS.pushesLeftTile)
                {

                    mDirection = EntityDirection.Right;

                }
                else if (Body.mPS.pushesRightTile)
                {
                    mDirection = EntityDirection.Left;
                }
 
                Body.mSpeed.x = GetMovementSpeed() * (int)mDirection;

                if(Target != null)
                {
                    float randomX = Random.Range(-0.5f, 0.5f);
                    mAttackManager.rangedAttacks[0].Activate(new Vector2(randomX, 1), Position);
                }

                break;
            case EnemyState.Jumping:
                if (Body.mPS.pushesBottom)
                {
                    mEnemyState = EnemyState.Moving;
                    break;
                }

                break;
        }

        base.EntityUpdate();

    }

}
