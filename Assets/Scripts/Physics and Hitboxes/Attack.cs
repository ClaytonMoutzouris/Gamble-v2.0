using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Range { Close, Near, Far };

public class Attack {

    //The owner of this attack
    public Entity mEntity;
    public AttackPrototype attackPrototype;
    public List<AttackTrait> traits;
    public List<StatusEffectType> statusEffects;
    public int damage;
    public float duration;
    public float elapsed;
    public float coolDown;
    public bool onCooldown;
    public float coolDownTimer = 0;
    public int startUpFrames = 0;
    public Vector2 attackOffset;

    //List of effects

    public bool mIsActive = false;

    public static Attack CrushAttack()
    {
        return new Attack(999);
    }

    public Attack(int damage)
    {
        this.damage = damage;
    }

    public Attack(Entity entity, AttackPrototype prototype)
    {
        attackPrototype = prototype;
        mEntity = entity;
        traits = prototype.abilities;
        duration = prototype.duration;
        damage = prototype.damage;
        coolDown = prototype.cooldown;
        attackOffset = prototype.offset;
    }



    public virtual void UpdateAttack()
    {
        if (coolDownTimer < coolDown)
            coolDownTimer += Time.deltaTime;



        if (!mIsActive)
            return;

        if(elapsed < duration)
        {
            elapsed += Time.deltaTime;
        }

        if(elapsed >= duration)
        {
            Deactivate();
        }
    }

    public virtual void SecondUpdate()
    {

    }

    public bool OnCooldown()
    {
        return coolDownTimer < coolDown;
    }
    
    public virtual void Activate()
    {
        if (mIsActive || OnCooldown())
            return;

        elapsed = 0;
        mIsActive = true;

    }

    public virtual void Deactivate()
    {
        elapsed = 0;
        coolDownTimer = 0;
        mIsActive = false;
    }

}

public class MeleeAttack : Attack
{
    public MeleeAttackObjectPrototype meleeObject;

    public MeleeAttack(Entity entity, MeleeAttackPrototype proto) : base(entity, proto)
    {
        meleeObject = proto.meleeObjectPrototype;
    }

    public MeleeAttack(Entity entity, MeleeWeapon meleeWeapon) :base(entity, meleeWeapon.attack)
    {
        meleeObject = meleeWeapon.attack.meleeObjectPrototype;

        //Override these values with the weapons values
        damage = meleeWeapon.damage;
        traits = meleeWeapon.weaponAbilities;
    }

    public override void Activate()
    {

        if (mIsActive || OnCooldown())
        {
            return;
        }

        base.Activate();


        MeleeAttackObject attack = new MeleeAttackObject(meleeObject, this);
        attack.Spawn(mEntity.Position);
        



    }


    public override void Deactivate()
    {
        base.Deactivate();


    }

    public override void UpdateAttack()
    {

        //hitbox.mCollisions.Clear();


        base.UpdateAttack();

    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();

    }

}


public class RangedAttack : Attack
{

    public int numberOfProjectiles;
    public float spreadAngle;
    public ProjectilePrototype projProto;

    /*
     * Constructor used for player attacks... fuck me I should just seperate these into different classes
     */

    public RangedAttack(Entity entity, RangedAttackPrototype prototype) : base(entity, prototype)
    {

        numberOfProjectiles = prototype.numberOfProjectiles;
        spreadAngle = prototype.spreadAngle;
        projProto = prototype.projectilePrototype;
    }

    public RangedAttack(Entity entity, RangedWeapon rangedWeapon) : base(entity, rangedWeapon.attack)
    {

        numberOfProjectiles = rangedWeapon.attack.numberOfProjectiles;
        spreadAngle = rangedWeapon.attack.spreadAngle;
        projProto = rangedWeapon.attack.projectilePrototype;

        //Override these values with the weapons values
        damage = rangedWeapon.damage;
        traits = rangedWeapon.weaponAbilities;
    }


    //These only cover the shooting animation really
    public override void Activate()
    {
        if (mIsActive || OnCooldown())
        {
            return;
        }

        base.Activate();

    }

    public void Activate(Vector2 direction, Vector2 spawnPoint)
    {
        if (mIsActive || OnCooldown())
        {
            return;
        }

        base.Activate();

        float interval = spreadAngle / numberOfProjectiles;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            Vector2 tempDir = direction;
            if(numberOfProjectiles > 1)
            {
                tempDir = tempDir.Rotate(-spreadAngle/2 + (interval*i));
            }

            tempDir.Normalize();

            Projectile shot = new Projectile(projProto, this, tempDir);
            shot.Spawn(spawnPoint + new Vector2(0, attackOffset.y) + (attackOffset * tempDir));
        }



    }


    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public override void Deactivate()
    {
        base.Deactivate();

    }
}