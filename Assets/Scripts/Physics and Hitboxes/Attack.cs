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

    public bool mIsActive = false;


    public Attack()
    {

    }

    public void UpdateAttack()
    {
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

    public void Hit(Hurtbox hurtbox)
    {
        hurtbox.GetHit(this);
    }
	
}

[System.Serializable]
public class MeleeAttack : Attack
{
    public Hitbox hitbox;

    public override void Activate()
    {
        base.Activate();

        hitbox.state = ColliderState.Open;
    }

    public override void Deactivate()
    {
        base.Deactivate();

        hitbox.state = ColliderState.Closed;
    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();

        hitbox.collider.Center = mEntity.Body.mAABB.Center;
        hitbox.collider.Scale = mEntity.Body.Scale;

        foreach(Hurtbox hurt in hitbox.colliders)
        {
            hurt.GetHit(this);
        }
    }

}

[System.Serializable]
public class RangedAttack : Attack
{
    public Bullet projectile;



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
