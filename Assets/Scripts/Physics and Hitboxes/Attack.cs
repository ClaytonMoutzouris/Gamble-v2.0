﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Range { Close, Near, Far };
[System.Serializable]
public class Attack {

    public Entity mEntity;

    public int damage;
    public float duration;
    public float elapsed;
    public float coolDown;
    public bool onCooldown;
    public float coolDownTimer = 0;
    //List of effects

    public bool mIsActive = false;

    public static Attack CrushAttack()
    {
        return new Attack(999);
    }

    public Attack(int damage)
    {
        this.damage = damage;
    }

    public Attack(Entity entity, float duration, int damage, float cd)
    {
        mEntity = entity;
        this.duration = duration;
        this.damage = damage;
        coolDown = cd;
    }

    public virtual void UpdateAttack()
    {
        if (coolDownTimer < coolDown)
            coolDownTimer += Time.deltaTime;



        if (!mIsActive)
            return;

        if(elapsed < duration)
        {
            elapsed += Time.deltaTime;
        }

        if(elapsed >= duration)
        {
            Deactivate();
        }
    }

    public virtual void SecondUpdate()
    {

    }

    public bool OnCooldown()
    {
        return coolDownTimer < coolDown;
    }
    
    public virtual void Activate()
    {
        if (mIsActive || OnCooldown())
            return;

        elapsed = 0;
        mIsActive = true;
    }

    public virtual void Deactivate()
    {
        elapsed = 0;
        coolDownTimer = 0;
        mIsActive = false;
    }



	
}

public class DashAttack : Attack
{
    public Hitbox hitbox;

    public DashAttack(Entity entity, float duration, int damage, float cd, Hitbox hit) : base(entity, duration, damage, cd)
    {
        hitbox = hit;
        hitbox.mState = ColliderState.Closed;
    }
    public override void Activate()
    {
        if (mIsActive || OnCooldown())
            return;

        base.Activate();

        hitbox.mState = ColliderState.Open;
        hitbox.mDealthWith.Clear();
        //hitbox.colliderType = ColliderType.Hitbox;
    }

    public override void Deactivate()
    {
        base.Deactivate();

        hitbox.mState = ColliderState.Closed;
        hitbox.mCollisions.Clear();
        hitbox.mDealthWith.Clear();

    }

    public override void UpdateAttack()
    {
        foreach (IHurtable hit in hitbox.mCollisions)
        {
            if (!hitbox.mDealthWith.Contains(hit))
            {
                hit.GetHurt(this);
                hitbox.mDealthWith.Add(hit);
            }

        }
        //hitbox.mCollisions.Clear();


        base.UpdateAttack();

        hitbox.UpdatePosition();
        CollisionManager.UpdateAreas(hitbox);
    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();

        hitbox.UpdatePosition();



    }
}

[System.Serializable]
public class MeleeAttack : Attack
{
    public Hitbox hitbox;
    public MeleeAttack(Entity entity, float duration, int damage, float cd, Hitbox hit) : base(entity, duration, damage, cd)
    {
        hitbox = hit;
        hitbox.mState = ColliderState.Closed;
    }
    public override void Activate()
    {
        if (mIsActive || OnCooldown())
            return;

        base.Activate();


        hitbox.mState = ColliderState.Open;
        hitbox.mDealthWith.Clear();
        //hitbox.colliderType = ColliderType.Hitbox;
    }

    public override void Deactivate()
    {
        base.Deactivate();

        hitbox.mState = ColliderState.Closed;
        hitbox.mCollisions.Clear();
        hitbox.mDealthWith.Clear();

    }

    public override void UpdateAttack()
    {

        foreach (IHurtable hit in hitbox.mCollisions)
        {
            if (!hitbox.mDealthWith.Contains(hit))
            {
                hit.GetHurt(this);
                hitbox.mDealthWith.Add(hit);
            }

        }
        //hitbox.mCollisions.Clear();


        base.UpdateAttack();

        hitbox.UpdatePosition();
        CollisionManager.UpdateAreas(hitbox);
    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();

        hitbox.UpdatePosition();



    }

}

[System.Serializable]
public class RangedAttack : Attack
{
    public Projectile projectile;

    public RangedAttack(Entity entity, float duration, int damage, float cd, Projectile proj) : base(entity, duration, damage, cd)
    {
        projectile = proj;
    }

    //These only cover the shooting animation really
    public override void Activate()
    {
        if (mIsActive || OnCooldown())
        {
            return;
        }

        base.Activate();

    }

    public void Activate(Vector2 direction)
    {
        if (mIsActive || OnCooldown())
        {
            return;
        }

        base.Activate();

        mEntity.Shoot(projectile, this, direction);

    }


    public override void Deactivate()
    {
        base.Deactivate();

    }
}
