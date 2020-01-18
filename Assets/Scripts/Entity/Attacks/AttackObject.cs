using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObject : Entity
{
    public Entity owner;
    public Hitbox hitbox;
    public Vector2 direction;
    public bool isAngled = false;
    public float mMaxTime = 10;
    public float mTimeAlive = 0;

    public AttackObject(AttackObjectPrototype proto, Attack attack, Vector2 direction) : base(proto)
    {
        mEntityType = EntityType.Projectile;
        mMaxTime = proto.maxTime;
        isAngled = proto.angled;
        this.direction = direction;



    }

    public override void EntityUpdate()
    {

        base.EntityUpdate();


    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();
        if(isAngled)
        {
            hitbox.UpdatePositionAndRotation(direction);

        }
        else
        {
            hitbox.UpdatePosition();

        }

    }


    public override void ActuallyDie()
    {
        CollisionManager.RemoveObjectFromAreas(hitbox);
        base.ActuallyDie();
    }

    public override void Crush()
    {
        base.Crush();
    }

    public override void Destroy()
    {
        base.Destroy();
    }

    public override void Die()
    {
        base.Die();
        hitbox.mState = ColliderState.Closed;

    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);
        if(prototype is AttackObjectPrototype proto)
        {
            if(proto.particleSystem != null)
            {
                Renderer.AddVisualEffect(proto.particleSystem, Vector2.zero);
            }
        }
        Renderer.SetSprite(prototype.sprite);

        if (prototype.animationController != null)
        {
            Renderer.Animator.runtimeAnimatorController = prototype.animationController;
        }
    }

    public virtual void Blocked()
    {
        Destroy();
    }
}
