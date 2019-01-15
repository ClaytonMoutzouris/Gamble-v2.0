﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity, IProjectile {

    //public Vector2 direction = Vector2.zero;
    public float mMaxTime = 10;
    public float mTimeAlive = 0;
    Hitbox mHitbox;

    public AudioClip fireSFX;
    //Does a bullet have a reference to an attack?
    //or does a bullet behave like an attack?
    private Entity owner;
    private Attack attack;
    public bool mPierce = false;
    public bool IgnoreGravity = true;

    public Attack Attack
    {
        get
        {
            return attack;
        }

        set
        {
            attack = value;
        }
    }

    public Entity Owner
    {
        get
        {
            return owner;
        }

        set
        {
            owner = value;
        }
    }

    public override void EntityInit()
    {
        base.EntityInit();

        Body = new PhysicsBody(this, new CustomAABB(transform.position, new Vector2(2.0f, 2.0f), new Vector2(0, 2.0f), new Vector3(1, 1, 1)));
        mHitbox = new Hitbox(this, new CustomAABB(transform.position, new Vector2(2.0f, 2.0f), new Vector2(0, 2.0f), new Vector3(1, 1, 1)));
        Body.mIsKinematic = true;
        //mMovingSpeed = 100;
        body.mIgnoresGravity = IgnoreGravity;
        mAudioSource.PlayOneShot(fireSFX);
    }

    public void SetInitialDirection(Vector3 direction)
    {
        Body.mSpeed = mMovingSpeed * direction;
    }

    public override void EntityUpdate()
    {
        foreach (IHurtable hit in mHitbox.mCollisions)
        {
            if (!mHitbox.mDealthWith.Contains(hit))
            {
                hit.GetHurt(Attack);
                if (!mPierce)
                {
                    mToRemove = true;
                    return;
                }
                mHitbox.mDealthWith.Add(hit);
            }

        }

        if (mTimeAlive >= mMaxTime || Body.mPS.pushesBottomTile || Body.mPS.pushesTopTile || Body.mPS.pushesLeftTile || Body.mPS.pushesRightTile)
        {
            mToRemove = true;
            return;
        }

        mTimeAlive += Time.deltaTime;

        //Body.mSpeed = mMovingSpeed * direction;

        base.EntityUpdate();

        CollisionManager.UpdateAreas(mHitbox);


    }

    public override void SecondUpdate()
    {
        //Should be in the habit of doing this first, i guess
        base.SecondUpdate();
        mHitbox.UpdatePosition();

    }

    public override void Die()
    {
        base.Die();

        mHitbox.mState = ColliderState.Closed;
    }

    public override void ActuallyDie()
    {
        CollisionManager.RemoveObjectFromAreas(mHitbox);

        base.ActuallyDie();

    }

}
