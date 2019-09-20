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

    public AttackObject(AttackObjectPrototype proto, Attack attack) : base(proto)
    {
        mEntityType = EntityType.Projectile;
        Body = new PhysicsBody(this, new CustomAABB(Position, proto.hitboxSize, new Vector2(0, proto.hitboxSize.y)));
        hitbox = new Hitbox(this, new CustomAABB(Position, proto.hitboxSize, new Vector2(0, proto.hitboxSize.y)));

        mMaxTime = proto.maxTime;
        isAngled = proto.angled;


    }

    public override void EntityUpdate()
    {

        base.EntityUpdate();


    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();
        hitbox.UpdatePosition();

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
        Renderer.SetSprite(prototype.sprite);
        if (prototype.animationController != null)
        {
            Renderer.Animator.runtimeAnimatorController = prototype.animationController;
        }
    }

}
