using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack {

    public Entity mEntity;

    public int damage;
    public float duration;
    public float elapsed;

    //List of effects

    public bool mIsActive = false;


    public Attack(Entity entity, float duration, int damage)
    {
        mEntity = entity;
        this.duration = duration;
        this.damage = damage;

    }

    public virtual void UpdateAttack()
    {
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
    
    public virtual void Activate()
    {
        if (mIsActive)
            return;

        elapsed = 0;
        mIsActive = true;
    }

    public virtual void Deactivate()
    {
        elapsed = 0;
        mIsActive = false;
    }



	
}

[System.Serializable]
public class MeleeAttack : Attack
{
    public Hitbox hitbox;
    public MeleeAttack(Entity entity, float duration, int damage, Hitbox hit) : base(entity, duration, damage)
    {
        hitbox = hit;
        hitbox.mState = ColliderState.Closed;
    }
    public override void Activate()
    {
        if (mIsActive)
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
    public Bullet projectile;

    public RangedAttack(Entity entity, float duration, int damage, Bullet proj) : base(entity, duration, damage)
    {
        projectile = proj;
    }

    //These only cover the shooting animation really
    public override void Activate()
    {
        if (mIsActive)
        {
            return;
        }

        base.Activate();

        mEntity.Shoot(projectile, this);
    }

    public override void UpdateAttack()
    {
        if (!mIsActive)
            return;

        base.UpdateAttack();
    }

    public override void Deactivate()
    {
        base.Deactivate();

    }
}
