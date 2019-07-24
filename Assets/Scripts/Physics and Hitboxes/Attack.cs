using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Range { Close, Near, Far };

public class Attack {

    public Entity mEntity;

    public int damage;
    public float duration;
    public float elapsed;
    public float coolDown;
    public bool onCooldown;
    public float coolDownTimer = 0;
    public int startUpFrames = 0;
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
        hitbox.mDealtWith.Clear();
        //hitbox.colliderType = ColliderType.Hitbox;
    }

    public override void Deactivate()
    {
        base.Deactivate();

        hitbox.mState = ColliderState.Closed;
        hitbox.mCollisions.Clear();
        hitbox.mDealtWith.Clear();

    }

    public override void UpdateAttack()
    {
        foreach (IHurtable hit in hitbox.mCollisions)
        {
            if (!hitbox.mDealtWith.Contains(hit))
            {
                hit.GetHurt(this);
                hitbox.mDealtWith.Add(hit);
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
        hitbox.mDealtWith.Clear();
        //hitbox.colliderType = ColliderType.Hitbox;
    }

    public override void Deactivate()
    {
        base.Deactivate();

        hitbox.mState = ColliderState.Closed;
        hitbox.mCollisions.Clear();
        hitbox.mDealtWith.Clear();

    }

    public override void UpdateAttack()
    {

        foreach (IHurtable hit in hitbox.mCollisions)
        {
            if (!hitbox.mDealtWith.Contains(hit))
            {
                hit.GetHurt(this);
                hitbox.mDealtWith.Add(hit);
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

public class RangedAttack : Attack
{
    public ProjectilePrototype projectile;
    public Vector2 projectileOffset;
    public int numberOfProjectiles;
    public float spreadAngle;

    public RangedAttack(Entity entity, float duration, int damage, float cd, int numProj, float spreadAngle, ProjectilePrototype proj, Vector2 offset) : base(entity, duration, damage, cd)
    {
        projectile = proj;
        projectileOffset = offset;
        numberOfProjectiles = numProj;
        this.spreadAngle = spreadAngle;
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

        float interval = spreadAngle / numberOfProjectiles;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            Vector2 tempDir = direction;
            if(numberOfProjectiles > 1)
            {
                tempDir = tempDir.Rotate(-spreadAngle/2 + (interval*i));
            }

            tempDir.Normalize();

            Projectile shot = new Projectile(projectile, this, tempDir);
            shot.Owner = mEntity;
            shot.Spawn(mEntity.Position + new Vector2(0, projectileOffset.y) + (projectileOffset * tempDir));
        }



    }


    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public override void Deactivate()
    {
        base.Deactivate();

    }
}