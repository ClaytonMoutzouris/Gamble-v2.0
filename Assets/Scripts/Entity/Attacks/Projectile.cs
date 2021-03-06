﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : AttackObject {

    public bool pierce;
    public bool collidesWithTiles = true;
    public int frameDelay = 0;
    public int framesAlive = 0;
    public bool isBouncy = false;
    public bool split = false;
    public bool homing = false;
    public Entity target = null;
    //Does a bullet have a reference to an attack?
    //or does a bullet behave like an attack?

    public Projectile(ProjectilePrototype proto, RangedAttack attack, Vector2 direction) : base(proto, attack, direction)
    {
        frameDelay = proto.frameDelay;
        this.collidesWithTiles = proto.collidesWithTiles;
        this.attack = attack;
        isAngled = proto.angled;
        //Body.mState = ColliderState.Closed;
        SetOwner(attack.mEntity);
        hitbox = new Hitbox(this, new CustomAABB(Position, proto.hitboxSize, new Vector2(0, 0)));
        hitbox.mState = ColliderState.Closed;
        Body = new PhysicsBody(this, new CustomAABB(Position, proto.bodySize, new Vector2(0, 0)));

        Body.mIsKinematic = true;
        //mMovingSpeed = 100;
        Body.mIgnoresOneWay = true;
        pierce = proto.pierce;
        isBouncy = proto.bouncy;

        Body.mIgnoresGravity = proto.ignoreGravity;

        mMovingSpeed = proto.speed;

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySingle(proto.spawnSFX);
        }
        //AudioSource.PlayClipAtPoint(sfx, Body.mPosition);
    }

    public void SetInitialDirection()
    {
        Body.mSpeed = mMovingSpeed*direction.normalized;

        if (isAngled)
        {

            if(direction.y > 0)
            {
                Renderer.transform.Rotate(0, 0, Vector2.Angle(Vector2.right, direction));
            }
            else
            {
                Renderer.transform.Rotate(0, 0, -Vector2.Angle(Vector2.right, direction));
            }
        }
    }

    public void SetHoming(Entity entity)
    {
        homing = true;
        target = entity;

    }

    public void UpdateDirection()
    {
        if(homing)
        {
            Vector2 homingDir = (target.Position - target.Position).normalized;
            Body.mSpeed = homingDir * mMovingSpeed;
        }
    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);

        SetInitialDirection();
        //mHitbox.mState = ColliderState.Open;
    }

    public override void EntityUpdate()
    {
        if (mTimeAlive >= mMaxTime)
        {
            mToRemove = true;
            return;
        }


        if (framesAlive >= frameDelay)
        {
            hitbox.mState = ColliderState.Open;

        }

        framesAlive++;



        if (hitbox.mState != ColliderState.Closed)
        {
            foreach (IHurtable hit in hitbox.mCollisions)
            {
                //Debug.Log("Something in the collisions");
                if (!hitbox.mDealtWith.Contains(hit))
                {
                    hit.GetHurt(attack);
                    if(owner is Entity entity)
                    {
                        int count = entity.abilities.Count;

                        for(int i = 0; i < count; i++)
                        {
                            entity.abilities[i].OnHitTrigger(this, hit);
                        }
                    }

                    if (!pierce)
                    {
                        mToRemove = true;
                        return;
                    }
                    hitbox.mDealtWith.Add(hit);

                }

            }

        }


        mTimeAlive += Time.deltaTime;
        //Debug.Log(mTimeAlive);
        //Debug.Log("Pushes Bottom: " + Body.mPS.pushesBottomTile);
        //Debug.Log("Pushes Top: " + Body.mPS.pushesTopTile);
        //Debug.Log("Pushes Left: " + Body.mPS.pushesLeftTile);
        //Debug.Log("Pushes Right: " + Body.mPS.pushesRightTile);
        if(isBouncy)
        {
            if(Body.mPS.pushesBottom || Body.mPS.pushesTop)
            {
                direction = new Vector2(direction.x, -direction.y);
                SetInitialDirection();

            }


            if (Body.mPS.pushesLeft || Body.mPS.pushesRight)
            {
                direction = new Vector2(-direction.x, direction.y);
                SetInitialDirection();

            }
        }

        UpdateDirection();

        if ((collidesWithTiles && !isBouncy && (Body.mPS.pushesBottom || Body.mPS.pushesTop || Body.mPS.pushesLeft || Body.mPS.pushesRight)))
        {
            mToRemove = true;
        }

        //Body.mSpeed = mMovingSpeed * direction;

        //Calling this pretty much just updates the body
        //Let's seee if we can make it update collision stuff aswell
        base.EntityUpdate();

        // 

        CollisionManager.UpdateAreas(hitbox);

    }

    public override void SecondUpdate()
    {
        //Should be in the habit of doing this first, i guess
        base.SecondUpdate();
        hitbox.UpdatePosition();
    }

    public override void Die()
    {
        base.Die();

    }

    public void GetReflected(Entity entity)
    {
        direction = direction.Rotate(180);
        SetOwner(entity);
        SetInitialDirection();
    }

}
