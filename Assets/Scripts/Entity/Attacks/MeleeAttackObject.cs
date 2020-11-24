using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackObject : AttackObject
{


    public MeleeAttackObject(MeleeAttackObjectPrototype proto, MeleeAttack attack, Vector2 direction) : base(proto, attack, direction)
    {
        hitbox = new Hitbox(this, new CustomAABB(Position, proto.hitboxSize, new Vector2(0, proto.hitboxSize.y)));

        this.attack = attack;
        mMaxTime = attack.duration;
        //Body.mState = ColliderState.Closed;
        SetOwner(attack.mEntity);
        isAngled = proto.angled;
        //mMovingSpeed = 100;
        

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySingle(proto.spawnSFX);
        }
    }

    public override void EntityUpdate()
    {
        if (hitbox.mState != ColliderState.Closed)
        {
            foreach (IHurtable hit in hitbox.mCollisions)
            {
                //Debug.Log("Something in the collisions");
                if (!hitbox.mDealtWith.Contains(hit))
                {
                    hit.GetHurt(attack);
                    if(owner is Entity entity)
                    {
                        int count = entity.abilities.Count;

                        for (int i = 0; i < count; i++)
                        {
                            entity.abilities[i].OnHitTrigger(this, hit);
                        }
                    }
                    hitbox.mDealtWith.Add(hit);

                }
            }
        }

        mTimeAlive += Time.deltaTime;

        if (mTimeAlive >= mMaxTime)
        {
            mToRemove = true;
            return;
        }


        base.EntityUpdate();


        Position = owner.Position + Vector2.up*attack.attackOffset.y + Vector2.right*attack.attackOffset.x*(int)owner.mDirection;

        CollisionManager.UpdateAreas(hitbox);
    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();
        //Position = owner.Position + attack.attackOffset * (Vector2.right * (int)owner.mDirection);
        Renderer.Sprite.flipX = (owner.mDirection == EntityDirection.Left);

    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint + attack.attackOffset);
        Renderer.SetAnimState("Attack");

    }

}
