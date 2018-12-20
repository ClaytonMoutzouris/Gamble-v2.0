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


    public Attack()
    {

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

    public void Hit(Hurtbox hurtbox)
    {
        hurtbox.GetHit(this);
    }

	
}

[System.Serializable]
public class MeleeAttack : Attack
{
    public CustomCollider2D hitbox;

    public override void Activate()
    {
        base.Activate();

        hitbox.mState = ColliderState.Open;
        hitbox.colliderType = ColliderType.Hitbox;
    }

    public override void Deactivate()
    {
        base.Deactivate();

        hitbox.mState = ColliderState.Closed;

    }

    public override void UpdateAttack()
    {
        foreach (CustomCollider2D hit in hitbox.mCollisions)
        {
            Debug.Log(hit.mEntity.name + " was hit by " + mEntity.name);
            //hurt.GetHit(this);
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
