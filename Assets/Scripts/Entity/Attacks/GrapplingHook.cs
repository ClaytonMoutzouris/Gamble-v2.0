using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : Entity
{
    Player owner;
    public float mTimeAlive = 0;
    public float maxDuration = 5;
    public Vector2 direction;
    public bool trigger = false;
    public bool miss = false;
    public Vector2 ownerOffset;
    HookShot gadget;

    public GrapplingHook(EntityPrototype proto, Player owner, Vector2 direction, Vector2 offset, HookShot gadget) : base(proto)
    {
        this.direction = direction;
        this.owner = owner;
        this.gadget = gadget;
        ownerOffset = offset;
        Body = new PhysicsBody(this, new CustomAABB(Position, proto.bodySize, new Vector2(0, 0)));
        Body.mIgnoresOneWay = true;
        mMovingSpeed = 750;
        Body.mIgnoresGravity = proto.ignoreGravity;
    }

    public void SetInitialDirection()
    {
        Body.mSpeed = mMovingSpeed * direction.normalized;

        if (direction.y > 0)
        {
            Renderer.transform.Rotate(0, 0, Vector2.Angle(Vector2.right, direction));
        }
        else
        {
            Renderer.transform.Rotate(0, 0, -Vector2.Angle(Vector2.right, direction));
        }
        
    }

    public void PullOwner()
    {
        if(Vector2.Distance(Position, owner.Position + ownerOffset) >= 16)
        {
            owner.Body.mSpeed = mMovingSpeed * (Position - (owner.Position + ownerOffset)).normalized;
        } else
        {
            Die();

        }

        if(owner.Body.mSpeed == Vector2.zero)
        {
            Die();
        }
    }

    public override void EntityUpdate()
    {

        if(trigger)
        {
            PullOwner();
        } else
        {
            if(Body.mPS.pushesBottomTile || Body.mPS.pushesTopTile
                || Body.mPS.pushesLeftTile || Body.mPS.pushesRightTile)
            {
                trigger = true;
                Body.mSpeed = Vector2.zero;
                owner.movementState = MovementState.Hooking;
            }
        }

        mTimeAlive += Time.deltaTime;

        if (mTimeAlive >= maxDuration)
        {
            
            Die();
            return;
        }

        //Body.mSpeed = mMovingSpeed * direction;

        //Calling this pretty much just updates the body
        //Let's seee if we can make it update collision stuff aswell
        base.EntityUpdate();

        // 


    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);

        SetInitialDirection();
        //mHitbox.mState = ColliderState.Open;
    }

    public override void Die()
    {
        base.Die();
        owner.movementState = MovementState.Jump;
        gadget.hook = null;
    }
}
