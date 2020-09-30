using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Companion : Entity
{

    public AttackManager mAttackManager;
    public Player owner;
    public Vector2 offset = new Vector2(32, 32);
    public bool copyAttack = false;

    public Companion(CompanionPrototype proto) : base(proto)
    {
        prototype = proto;

        mMovingSpeed = proto.movementSpeed;
        mCollidesWith = proto.CollidesWith;
        Body.mIgnoresGravity = proto.ignoreGravity;
        mAttackManager = new AttackManager(this);
        mAttackManager.rangedAttacks.Add(new RangedAttack(this, (RangedAttackPrototype)proto.attack));
        copyAttack = proto.copyAttack;
    }

    public virtual void SetOwner(Player player)
    {
        owner = player;
        player.companionManager.AddCompanion(this);
        if(copyAttack)
        {
            mAttackManager.rangedAttacks[0] = new RangedAttack(this, (RangedAttackPrototype)owner.AttackManager.rangedAttacks[0].attackPrototype);
        }

    }

    public override void EntityUpdate()
    {
        Position = Vector2.Lerp(Position, owner.Position + offset, mMovingSpeed / 10);

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
        base.Spawn(spawnPoint + offset);
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
