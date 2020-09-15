using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Companion : Entity
{

    public AttackManager mAttackManager;
    public Player owner;

    public Companion(CompanionPrototype proto) : base(proto)
    {
        prototype = proto;

        mMovingSpeed = proto.movementSpeed;
        mCollidesWith = proto.CollidesWith;
        Body = new PhysicsBody(this, new CustomAABB(Position, prototype.bodySize, new Vector2(0, prototype.bodySize.y)));
        Body.mIgnoresGravity = proto.ignoreGravity;
        mAttackManager = new AttackManager(this);
        mAttackManager.rangedAttacks.Add(new RangedAttack(this, (RangedAttackPrototype)proto.attack));
    }

    public override void EntityUpdate()
    {
        Position = Vector2.Lerp(Position, owner.Position + new Vector2(32, 32), mMovingSpeed / 10);

        if (owner.AttackManager.rangedAttacks[0].mIsActive)
        {

            mAttackManager.rangedAttacks[0].Activate(owner.GetAimRight(), Position);

        }

        mAttackManager.UpdateAttacks();

        base.EntityUpdate();





    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();

        mAttackManager.SecondUpdate();

    }

    public override void ShowFloatingText(string text, Color color)
    {
        base.ShowFloatingText(text, color);
    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);
        Renderer.SetSprite(prototype.sprite);
        if (prototype.animationController != null)
        {
            Renderer.Animator.runtimeAnimatorController = prototype.animationController;
        }


        //Renderer.SetSprite(prototype.)

    }

    public override string ToString()
    {
        return base.ToString();
    }

    public Entity GetEntity()
    {
        return this;
    }
}
