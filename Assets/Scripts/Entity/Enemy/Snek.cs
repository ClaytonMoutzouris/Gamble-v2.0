using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snek : Enemy
{
    public Entity host;
    public Vector2 hostOffset;

    public Snek(EnemyPrototype proto) : base(proto)
    {

    }

    public override void EntityUpdate()
    {
    
        switch(host != null)
        {
            case true:
                HostUpdate();
                break;

            case false:
                NoHostUpdate();
                break;
        }



        base.EntityUpdate();


    }

    public void HostUpdate()
    {
        Body.mSpeed = Vector2.zero;
        Position = host.Position + hostOffset;
        mAttackManager.meleeAttacks[0].Activate();

    }

    public void NoHostUpdate()
    {
        foreach (CollisionData col in Body.mCollisions)
        {
            if (col.other.mEntity is Player)
            {
                host = col.other.mEntity;
                hostOffset = (col.pos1-col.pos2)*0.5f;
                ignoreTilemap = true;
                Body.mState = ColliderState.Closed;
            }
        }

        EnemyBehaviour.CheckForTargets(this);

        if (Target != null)
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

            EnemyBehaviour.MoveHorizontal(this);

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
    }


}
