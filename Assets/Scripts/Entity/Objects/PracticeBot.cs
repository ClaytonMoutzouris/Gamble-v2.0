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

        if (damage <= 0)
        {
            damage = 0;
        }
        else
        {
            //Crits
            if (attack.mEntity != null && Random.Range(0, 100) < 5 + attack.mEntity.mStats.GetStat(StatType.Luck).value)
            {
                damage *= 2;
                ShowFloatingText(damage.ToString(), Color.yellow, 2, 20, 2);
            }
            else
            {
                ShowFloatingText(damage.ToString(), Color.white);
            }

            health.LoseHP(damage);
        }

        if (health.currentHealth == 0)
        {
            health.GainHP(health.maxHealth);
            Color timeColor = Color.white;
            float completionTime = (Time.time - firstDamageTimeStamp);
            switch (completionTime)
            {
                case float n when (n >= 10):
                    timeColor = Color.red;
                    break;
                case float n when (n >= 5):
                    timeColor = Color.yellow;
                    break;
                case float n when (n >= 0):
                    timeColor = Color.green;
                    break;
                default:
                    break;

            }

            ShowFloatingText(completionTime.ToString(), timeColor);

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
