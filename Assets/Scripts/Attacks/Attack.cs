using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Range { Close, Near, Far };

public enum AttackState { Inactive, Startup, Active, Exit };

public class Attack {

    //The owner of this attack
    public Entity mEntity;
    public AttackPrototype attackPrototype;
    public List<AttackTrait> traits;
    public List<StatusEffectType> statusEffects;
    public int baseDamage;
    public float duration;
    public float elapsedFrames;
    public float coolDown;
    public bool onCooldown;
    public float coolDownTimer = 0;
    public int startUpFrames = 0;
    public Vector2 attackOffset;

    //List of effects
    public AttackState state = AttackState.Inactive;
    public bool mIsActive = false;

    public static Attack CrushAttack()
    {
        return new Attack(999);
    }

    public static Attack ProtectedCrushAttack()
    {
        return new Attack(1);
    }

    public Attack(int damage)
    {
        this.baseDamage = damage;
    }

    public Attack(Entity entity, AttackPrototype prototype)
    {
        attackPrototype = prototype;
        mEntity = entity;
        traits = prototype.abilities;
        duration = prototype.duration;
        baseDamage = prototype.damage;
        coolDown = prototype.cooldown;
        attackOffset = prototype.offset;
        startUpFrames = prototype.startUpFrames;
    }



    public virtual void UpdateAttack()
    {

        if (coolDownTimer < coolDown)
            coolDownTimer += Time.deltaTime;
        

        if (!mIsActive)
            return;

        elapsedFrames += 1;


        if (elapsedFrames >= duration)
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

        elapsedFrames = 0;
        mIsActive = true;

    }

    public virtual void Deactivate()
    {
        elapsedFrames = 0;
        coolDownTimer = 0;
        mIsActive = false;
    }

    public int GetDamage()
    {
        int damage = baseDamage; ;
        if (mEntity is Player player)
        {
            damage += player.mStats.getStat(StatType.Attack).GetValue();
        }

        return damage;

    }

}

public class MeleeAttack : Attack
{
    public MeleeAttackObjectPrototype meleeObject;

    public MeleeAttackObject attack;

    public MeleeAttack(Entity entity, MeleeAttackPrototype proto) : base(entity, proto)
    {
        meleeObject = proto.meleeObjectPrototype;
    }

    public MeleeAttack(Entity entity, MeleeWeapon meleeWeapon) :base(entity, meleeWeapon.attack)
    {
        meleeObject = meleeWeapon.attack.meleeObjectPrototype;

        //Override these values with the weapons values
        baseDamage = meleeWeapon.damage;
        traits = meleeWeapon.weaponAbilities;
    }

    public override void Activate()
    {

        if (mIsActive || OnCooldown())
        {
            return;
        }

        base.Activate();


        attack = new MeleeAttackObject(meleeObject, this, Vector2.right);
        attack.Spawn(mEntity.Position);
        



    }


    public override void Deactivate()
    {
        if (attack != null)
        {
            attack.Destroy();
        }
        base.Deactivate();

    }

    public override void UpdateAttack()
    {
        if (attack != null)
        {
            //hitbox.mCollisions.Clear();
            if (elapsedFrames < startUpFrames)
            {
                attack.hitbox.mState = ColliderState.Closed;
            }
            else
            {
                attack.hitbox.mState = ColliderState.Open;

            }
        }

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
        baseDamage = rangedWeapon.damage;
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