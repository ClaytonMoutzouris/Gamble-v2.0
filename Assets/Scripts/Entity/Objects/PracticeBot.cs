using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeBot : Entity, IHurtable
{
    Hurtbox hurtbox;
    Health health;
    float firstDamageTimeStamp;

    public PracticeBot(EntityPrototype proto) : base(proto)
    {

        HurtBox = new Hurtbox(this, new CustomAABB(Position, proto.bodySize, new Vector2(0, proto.bodySize.y)));
        HurtBox.UpdatePosition();
        Body.mIgnoresGravity = proto.ignoreGravity;
        mCollidesWith = proto.CollidesWith;
        Body.mIsKinematic = true;

        health = new Health(this, 100);
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
        if(health.currentHealth == health.maxHealth)
        {
            firstDamageTimeStamp = Time.time;
        }

        int damage = attack.GetDamage();

        if (damage < 0)
        {
            damage = 0;
        }
        health.LoseHP(damage);
        ShowFloatingText(damage.ToString(), Color.white);

        if (health.currentHealth == 0)
        {
            health.GainHP(health.maxHealth);
            ShowFloatingText((Time.time - firstDamageTimeStamp).ToString(), Color.white);

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
