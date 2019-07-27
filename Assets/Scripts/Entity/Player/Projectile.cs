using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity, IProjectile {

    Hitbox mHitbox;
    ProjectilePrototype prototype;
    Vector2 direction;
    public float mMaxTime = 10;
    public float mTimeAlive = 0;
    //Does a bullet have a reference to an attack?
    //or does a bullet behave like an attack?
    private Entity owner;
    private Attack attack;


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

    public Projectile(ProjectilePrototype proto, RangedAttack rAttack, Vector2 direction) : base(proto)
    {
        prototype = proto;
        owner = rAttack.mEntity;
        mEntityType = EntityType.Projectile;
        mMaxTime = proto.maxTime;

        mHitbox = new Hitbox(this, new CustomAABB(Position, prototype.bodySize, new Vector2(0, 0)));
        //mHitbox.mState = ColliderState.Closed;
        Body = new PhysicsBody(this, new CustomAABB(Position, prototype.bodySize, new Vector2(0, 0)));
        //Body.mState = ColliderState.Closed;

        Body.mIsKinematic = true;
        //mMovingSpeed = 100;
        Body.mIgnoresGravity = prototype.ignoreGravity;
        Body.mIgnoresOneWay = true;

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySingle(prototype.sfx);
        }

        attack = rAttack;
        mMovingSpeed = prototype.speed;
        this.direction = direction;
        //AudioSource.PlayClipAtPoint(sfx, Body.mPosition);
    }

    public void SetInitialDirection()
    {
        Body.mSpeed = mMovingSpeed*direction;

        if (prototype.angled)
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

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);
        Renderer.SetSprite(prototype.sprite);
        if (prototype.animationController != null)
        {
            Renderer.Animator.runtimeAnimatorController = prototype.animationController;
        }
        SetInitialDirection();
        //mHitbox.mState = ColliderState.Open;

    }

    public override void EntityUpdate()
    {

        foreach (IHurtable hit in mHitbox.mCollisions)
        {
            //Debug.Log("Something in the collisions");
            if (!mHitbox.mDealtWith.Contains(hit))
            {
                hit.GetHurt(Attack);
                if (!prototype.pierce)
                {
                    mToRemove = true;
                    return;
                }
                mHitbox.mDealtWith.Add(hit);
            }

        }



        mTimeAlive += Time.deltaTime;

        //Debug.Log("Pushes Bottom: " + Body.mPS.pushesBottomTile);
        //Debug.Log("Pushes Top: " + Body.mPS.pushesTopTile);
        //Debug.Log("Pushes Left: " + Body.mPS.pushesLeftTile);
        //Debug.Log("Pushes Right: " + Body.mPS.pushesRightTile);

        if (mTimeAlive >= mMaxTime || Body.mPS.pushesBottomTile || Body.mPS.pushesTopTile || Body.mPS.pushesLeftTile || Body.mPS.pushesRightTile)
        {
            mToRemove = true;
            return;
        }

        //Body.mSpeed = mMovingSpeed * direction;

        //Calling this pretty much just updates the body
        //Let's seee if we can make it update collision stuff aswell
        base.EntityUpdate();

        // 

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
