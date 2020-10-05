using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iceblock : Entity, IHurtable
{
    public bool broken = false;
    public bool dropped = false;
    private Hurtbox hurtbox;
    public Health health;

    public Iceblock(EntityPrototype proto) : base(proto)
    {
        Body.mIsKinematic = false;

        mEntityType = EntityType.Obstacle;
        HurtBox = new Hurtbox(this, new CustomAABB(Position, prototype.bodySize, new Vector2(0, prototype.bodySize.y)));
        HurtBox.UpdatePosition();

        health = new Health(this, 25);
    }

    public Hurtbox HurtBox { get => hurtbox; set => hurtbox = value; }

    public override void ActuallyDie()
    {
        CollisionManager.RemoveObjectFromAreas(HurtBox);

        base.ActuallyDie();
    }

    public override void Destroy()
    {
        base.Destroy();
    }

    public override void Die()
    {
        base.Die();

        HurtBox.mState = ColliderState.Closed;

    }

    public override void EntityUpdate()
    {
        if (!broken && Body.mPS.pushesBottom)
        {
            broken = true;
            Renderer.SetAnimState("IceblockBroken");
        }

        base.EntityUpdate();

        if (Body.mPS.pushesBottom)
        {
            Body.mIsKinematic = false;

        } else
        {
            Body.mIsKinematic = true;
        }

        CollisionManager.UpdateAreas(HurtBox);

    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public Entity GetEntity()
    {
        return this;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public void GetHurt(Attack attack)
    {

        int damage = attack.GetDamage();

        if (damage < 0)
        {
            damage = 0;
        }
        health.LoseHP(damage);
        ShowFloatingText(damage.ToString(), Color.white);

        if (health.currentHealth == 0)
        {
            Die();
        }

    }


    public override void SecondUpdate()
    {
        base.SecondUpdate();

        HurtBox.UpdatePosition();

    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public void GainLife(int health, bool fromTrigger)
    {
        //throw new System.NotImplementedException();
    }
}
