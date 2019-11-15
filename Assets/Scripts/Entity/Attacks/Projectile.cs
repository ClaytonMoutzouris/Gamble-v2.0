using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : AttackObject {

    public RangedAttack attack;
    public bool pierce;
    //Does a bullet have a reference to an attack?
    //or does a bullet behave like an attack?

    public Projectile(ProjectilePrototype proto, RangedAttack attack, Vector2 direction) : base(proto, attack, direction)
    {
        this.attack = attack;
        //Body.mState = ColliderState.Closed;
        owner = this.attack.mEntity;
        hitbox = new Hitbox(this, new CustomAABB(Position, proto.hitboxSize, new Vector2(0, 0)));
        Body = new PhysicsBody(this, new CustomAABB(Position, proto.hitboxSize, new Vector2(0, 0)));

        Body.mIsKinematic = true;
        //mMovingSpeed = 100;
        Body.mIgnoresOneWay = true;
        pierce = proto.pierce;
        Body.mIgnoresGravity = proto.ignoreGravity;


        mMovingSpeed = proto.speed;

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySingle(proto.sfx);
        }
        //AudioSource.PlayClipAtPoint(sfx, Body.mPosition);
    }

    public void SetInitialDirection()
    {
        Body.mSpeed = mMovingSpeed*direction.normalized;

        if (attack.attackPrototype)
        {
            if(direction.y > 0)
            {
                Renderer.transform.Rotate(0, 0, Vector2.Angle(Vector2.right, direction));
            }
            else
            {
                Renderer.transform.Rotate(0, 0, -Vector2.Angle(Vector2.right, direction));
            }
        }
    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);

        SetInitialDirection();
        //mHitbox.mState = ColliderState.Open;
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
                    if (attack.traits.Contains(AttackTrait.Exploding))
                    {
                        WeaponAbilities.Explode(owner, Position);
                    }
                    if (attack.traits.Contains(AttackTrait.Split))
                    {
                        WeaponAbilities.Split(this, hit);
                    }

                    if (!pierce)
                    {
                        mToRemove = true;
                        return;
                    }
                    hitbox.mDealtWith.Add(hit);

                }

            }
        }


        mTimeAlive += Time.deltaTime;

        //Debug.Log("Pushes Bottom: " + Body.mPS.pushesBottomTile);
        //Debug.Log("Pushes Top: " + Body.mPS.pushesTopTile);
        //Debug.Log("Pushes Left: " + Body.mPS.pushesLeftTile);
        //Debug.Log("Pushes Right: " + Body.mPS.pushesRightTile);

        if (mTimeAlive >= mMaxTime || Body.mPS.pushesBottomTile || Body.mPS.pushesTopTile || Body.mPS.pushesLeftTile || Body.mPS.pushesRightTile)
        {
            mToRemove = true;
            return;
        }

        //Body.mSpeed = mMovingSpeed * direction;

        //Calling this pretty much just updates the body
        //Let's seee if we can make it update collision stuff aswell
        base.EntityUpdate();

        // 

        CollisionManager.UpdateAreas(hitbox);

    }

    public override void SecondUpdate()
    {
        //Should be in the habit of doing this first, i guess
        base.SecondUpdate();
        hitbox.UpdatePosition();
    }

    public override void Die()
    {
        base.Die();

    }

}
