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


    public Attack(Entity entity, float duration)
    {
        mEntity = entity;
        this.duration = duration;

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
    public MeleeAttack(Entity entity, float duration, Hitbox hit) : base(entity, duration)
    {
        hitbox = hit;
        hitbox.mState = ColliderState.Closed;
    }
    public override void Activate()
    {
        base.Activate();

        hitbox.mState = ColliderState.Open;
        //hitbox.colliderType = ColliderType.Hitbox;
    }

    public override void Deactivate()
    {
        base.Deactivate();

        hitbox.mState = ColliderState.Closed;

    }

    public override void UpdateAttack()
    {
        foreach (IHurtable hit in hitbox.mCollisions)
        {
                hit.GetHurt(this);
           
        }
        hitbox.mCollisions.Clear();


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

    public RangedAttack(Entity entity, float duration, Bullet proj) : base(entity, duration)
    {
        projectile = proj;
    }

    public override void Activate()
    {
        base.Activate();

        mEntity.Shoot(projectile);
    }

    public override void Deactivate()
    {
        base.Deactivate();

    }
}
