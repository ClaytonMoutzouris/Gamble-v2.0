using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackObject : AttackObject
{
    public MeleeAttack attack;


    public MeleeAttackObject(MeleeAttackObjectPrototype proto, MeleeAttack attack) : base(proto, attack)
    {
        hitbox = new Hitbox(this, new CustomAABB(Position, proto.hitboxSize, new Vector2(0, proto.hitboxSize.y)));
        Body = new PhysicsBody(this, new CustomAABB(Position, proto.hitboxSize, new Vector2(0, proto.hitboxSize.y)));

        this.attack = attack;
        mMaxTime = attack.duration;
        //Body.mState = ColliderState.Closed;
        owner = attack.mEntity;
        isAngled = proto.angled;
        //mMovingSpeed = 100;

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySingle(attack.attackPrototype.sfx);
        }
    }

    public override void EntityUpdate()
    {
        foreach (IHurtable hit in hitbox.mCollisions)
        {
            //Debug.Log("Something in the collisions");
            if (!hitbox.mDealtWith.Contains(hit))
            {
                hit.GetHurt(attack);
                hitbox.mDealtWith.Add(hit);

            }
        }

        mTimeAlive += Time.deltaTime;

        if (mTimeAlive >= mMaxTime)
        {
            mToRemove = true;
            return;
        }


        base.EntityUpdate();


        CollisionManager.UpdateAreas(hitbox);
        Position = owner.Position + attack.attackOffset*(Vector2.right*(int)owner.mDirection);
    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();
        Position = owner.Position;

    }


    public override void ActuallyDie()
    {
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
    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint + attack.attackOffset);
    }

}
