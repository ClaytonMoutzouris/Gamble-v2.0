using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogLegs : Enemy
{

    public FrogLegs(EnemyPrototype proto) : base(proto)
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
                //If they can't proceed horizontally, try jumping.
                if (Body.mPS.pushesRightTile && Body.mPS.pushesBottom)
                {
                    EnemyBehaviour.Jump(this, jumpHeight);
                }
                //body.mSpeed.x = mMovingSpeed;
                mDirection = EntityDirection.Right;
            }
            else
            {
                if (Body.mPS.pushesLeftTile && Body.mPS.pushesBottom)
                {
                    EnemyBehaviour.Jump(this, jumpHeight);
                }
                mDirection = EntityDirection.Left;
            }
        }
        else
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

        EnemyBehaviour.MoveHorizontal(this);
        base.EntityUpdate();

    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);
    }

}
