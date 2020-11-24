using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Companion : Entity
{

    public AttackManager mAttackManager;
    public Player owner;
    public Vector2 offset = new Vector2(8, 32);
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
        if(copyAttack && owner.Equipment.GetSlot(EquipmentSlotType.Mainhand).GetContents() is RangedWeapon weapon)
        {
            mAttackManager.rangedAttacks[0] = new RangedAttack(this, ScriptableObject.Instantiate(weapon.attack));
        }

    }

    public override void EntityUpdate()
    {
        Position = Vector2.Lerp(Position, owner.Position + offset, mMovingSpeed / 10);

        mAttackManager.UpdateAttacks();

        base.EntityUpdate();





    }

    public void Fire()
    {
        mAttackManager.rangedAttacks[0].Activate(owner.GetAimRight(), Position);
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
