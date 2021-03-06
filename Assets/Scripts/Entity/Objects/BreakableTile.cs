﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableTile : Entity, IHurtable
{
    Hurtbox hurtbox;
    Health health;

    public BreakableTile(EntityPrototype proto) : base(proto)
    {

        Body = new PhysicsBody(this, new CustomAABB(Position, proto.bodySize, Vector2.zero));
        HurtBox = new Hurtbox(this, new CustomAABB(Position, proto.bodySize, Vector2.zero));
        HurtBox.UpdatePosition();
        Body.mIgnoresGravity = proto.ignoreGravity;
        mCollidesWith = proto.CollidesWith;
        Body.mIsKinematic = true;

        health = new Health(this, 15);

    }

    public Hurtbox HurtBox { get => hurtbox; set => hurtbox = value; }

    public override void EntityUpdate()
    {
        base.EntityUpdate();

        CollisionManager.UpdateAreas(HurtBox);

    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);
        Renderer.SetSprite(MapManager.instance.mCurrentMap.Data.tileSprites[1]);

    }

    public override void SecondUpdate()
    {

        base.SecondUpdate();
        HurtBox.UpdatePosition();

    }

    public Entity GetEntity()
    {
        return this;
    }

    public void GetHurt(Attack attack)
    {
        if(!(attack.mEntity is Player))
        {
            return;
        }
        int damage = attack.GetDamage();

        if (damage <= 0)
        {
            damage = 0;
        }
        else
        {
            health.LoseHP(damage);
            ShowFloatingText(damage.ToString(), attack.GetColorForDamageType());
        }


        if (health.currentHealth == 0)
        {
            Die();
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        HurtBox.mState = ColliderState.Closed;
    }

    public override void ActuallyDie()
    {

        CollisionManager.RemoveObjectFromAreas(HurtBox);

        //Do other stuff first because the base destroys the object
        base.ActuallyDie();
    }

    public void GainLife(int health, bool fromTrigger)
    {
        //throw new System.NotImplementedException();
    }
}
