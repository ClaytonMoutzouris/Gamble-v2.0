using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableTile : Entity, IHurtable
{
    Hurtbox hurtbox;
    Health health;

    public BreakableTile(EntityPrototype proto) : base(proto)
    {
        mEntityType = EntityType.Object;

        Body = new PhysicsBody(this, new CustomAABB(Position, proto.bodySize, Vector2.zero));
        HurtBox = new Hurtbox(this, new CustomAABB(Position, proto.bodySize, Vector2.zero));
        HurtBox.UpdatePosition();

        Body.mIsKinematic = false;

        health = new Health(this, 15);
    }

    public Hurtbox HurtBox { get => hurtbox; set => hurtbox = value; }

    public override void EntityUpdate()
    {
        base.EntityUpdate();

        CollisionManager.UpdateAreas(HurtBox);

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
        health.LoseHP(attack.GetDamage());
    }

}
