using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowdrift : Enemy
{

    public Snowdrift(EnemyPrototype proto) : base(proto)
    {

    }

    public override void EntityUpdate()
    {

        if (Hostility == Hostility.Hostile)
        {
            EnemyBehaviour.CheckForTargets(this);
        }

        if (Target != null && !Target.IsDead)
        {

            if (Target.Position.x > Position.x)
            {
                mDirection = EntityDirection.Right;
            }
            else
            {
                mDirection = EntityDirection.Left;

            }
            EnemyBehaviour.MoveHorizontal(this);

        } else
        {
            if (Body.mPS.pushedLeftTile)
            {
                //Renderer.Sprite.flipX = true;
                mDirection = EntityDirection.Right;

            }
            else if (Body.mPS.pushedRightTile)
            {
                //Renderer.Sprite.flipX = true;
                mDirection = EntityDirection.Left;
            }
        }

        base.EntityUpdate();

    }
}
