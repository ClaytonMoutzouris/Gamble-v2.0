using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponClassType { Pistol, Launcher, Automatic, Thrower, Charge, Shotgun, Fists, Sword, Axe, All = -1 };

public class RangedWeapon : Weapon
{
    public RangedAttackPrototype attack;
    [HideInInspector]
    public int ammunitionCount = 5;
    float reloadTimeStamp;
    public bool reloading = false;
    //Measured in shots per second
    float fireTimeStamp;
    public ProjectilePrototype projProto;
    public DamageType damageType = DamageType.Physical;

    #region baseAttributes
    public List<WeaponAttribute> baseAttributes = new List<WeaponAttribute>{
        new WeaponAttribute(WeaponAttributesType.NumProjectiles, 1),
        new WeaponAttribute(WeaponAttributesType.SpreadAngle, 0),
        new WeaponAttribute(WeaponAttributesType.AmmoCapacity, 5),
        new WeaponAttribute(WeaponAttributesType.ReloadTime, 3),
        new WeaponAttribute(WeaponAttributesType.Damage, 5),
        new WeaponAttribute(WeaponAttributesType.FireRate, 3),
        new WeaponAttribute(WeaponAttributesType.ProjectileSpeed, 100),

    };
    #endregion


    public override void Initialize()
    {
        base.Initialize();
        projProto = Instantiate(projProto);
        attributes = new WeaponAttributes();
        attributes.SetStats(baseAttributes);
        ammunitionCount = (int)attributes.GetAttribute(WeaponAttributesType.AmmoCapacity).GetValue();
        projProto.speed = (int)attributes.GetAttribute(WeaponAttributesType.ProjectileSpeed).GetValue();

    }

    public override void OnEquip(Player player)
    {

        base.OnEquip(player);

        ammunitionCount = (int)attributes.GetAttribute(WeaponAttributesType.AmmoCapacity).GetValue();

        if (mSlot == EquipmentSlotType.Mainhand)
        {
            equipped.AttackManager.rangedAttacks[0] = new RangedAttack(equipped, attack);
            foreach(Companion companion in player.companionManager.Companions)
            {
                if(companion.copyAttack)
                {
                    companion.mAttackManager.rangedAttacks[0] = new RangedAttack(companion, attack);
                }
            }
            ((PlayerRenderer)equipped.Renderer).rangedWeapon.sprite = sprite;


        }
        else
        {
            //player.AttackManager.rangedAttacks[1] = new RangedAttack(player, attack.duration, attack.damage, attack.cooldown, attack.projectile, attack.offset);
            //((PlayerRenderer)player.Renderer).weapon.sprite = sprite;
        }
    }

    public override bool Attack()
    {
        bool outcome = false;
        /*
        if(reloading && Time.time > reloadTimeStamp+reloadTime)
        {
            reloading = false;
            ammunitionCount = ammunitionCapacity;
            Debug.Log("Reloading Complete");

        }
        */
        if (!reloading && ammunitionCount > 0)
        {

            if(Time.time > fireTimeStamp+(1/ attributes.GetAttribute(WeaponAttributesType.FireRate).GetValue()))
            {


                ammunitionCount--;

                //Debug.Log("Fire " + (ammunitionCapacity - ammunitionCount) + " (Last shot at: " + fireTimeStamp);

                fireTimeStamp = Time.time;

                float interval = attributes.GetAttribute(WeaponAttributesType.SpreadAngle).GetValue() / attributes.GetAttribute(WeaponAttributesType.NumProjectiles).GetValue();

                for (int i = 0; i < attributes.GetAttribute(WeaponAttributesType.NumProjectiles).GetValue(); i++)
                {

                    Vector2 tempDir = equipped.GetAimRight().normalized;

                    if (attributes.GetAttribute(WeaponAttributesType.NumProjectiles).GetValue() > 1)
                    {
                        tempDir = tempDir.Rotate(-attributes.GetAttribute(WeaponAttributesType.SpreadAngle).GetValue() / 2 + (interval * i));
                    }

                    tempDir.Normalize();
                    RangedAttackPrototype attackProto = Instantiate(attack);
                    attackProto.damage = (int)attributes.GetAttribute(WeaponAttributesType.Damage).GetValue();
                    attackProto.damageType = damageType;
                    ProjectilePrototype tempProto = Instantiate(projProto);
                    tempProto.speed = (int)attributes.GetAttribute(WeaponAttributesType.ProjectileSpeed).GetValue();
                    Projectile shot = new Projectile(tempProto, new RangedAttack(equipped, attackProto), tempDir);
                    shot.Spawn(equipped.Position + new Vector2(0, attackProto.offset.y) + (attackProto.offset * tempDir.normalized));

                }

                foreach(Companion companion in equipped.companionManager.Companions)
                {
                    companion.Fire();
                }
                //Actually fire here
                outcome = true;
            }

            if (ammunitionCount <= 0)
            {
                reloadTimeStamp = Time.time;
                reloading = true;
                Debug.Log("Reloading");

            }
        }

        return outcome;
    }

    public override void OnUpdate()
    {
        
        if (reloading && Time.time > reloadTimeStamp + attributes.GetAttribute(WeaponAttributesType.ReloadTime).GetValue())
        {
            reloading = false;
            ammunitionCount = (int)attributes.GetAttribute(WeaponAttributesType.AmmoCapacity).GetValue();
            Debug.Log("Reloading Complete");

        }
    }

    public void Reload()
    {
        if (reloading && Time.time > reloadTimeStamp + attributes.GetAttribute(WeaponAttributesType.ReloadTime).GetValue())
        {
            reloading = false;
            ammunitionCount = (int)attributes.GetAttribute(WeaponAttributesType.AmmoCapacity).GetValue();
            Debug.Log("Reloading Complete");

        }
    }

    public void ChargeUp()
    {

    }

    public override void OnUnequip(Player player)
    {
        base.OnUnequip(player);

        player.AttackManager.rangedAttacks[0] = player.defaultRanged;
        ((PlayerRenderer)player.Renderer).rangedWeapon.sprite = null;

    }

    public override string GetTooltip()
    {
        string tooltip = base.GetTooltip();
        tooltip += "\n<color=silver>" + ammoType.ToString()+ "</color>";
        tooltip += "\n<color=white>" + attributes.GetAttribute(WeaponAttributesType.Damage).GetValue() + " Damage</color>";
        tooltip += "\n<color=white>" + AttackSpeedToString() + "</color>";

        return tooltip;
    }

    public string AttackSpeedToString()
    {
        string speed = "";
        float fireRate = attributes.GetAttribute(WeaponAttributesType.FireRate).GetValue();
        if (fireRate > 10)
        {
            speed = "Very Fast";
        }
        else if (fireRate < 10 && fireRate >= 6)
        {
            speed = "Fast";
        }
        else if (fireRate < 6 && fireRate >= 3)
        {
            speed = "Medium";
        }
        else if (fireRate < 3 && fireRate >= 1)
        {
            speed = "Slow";
        }
        else if (fireRate < 1)
        {
            speed = "Very Slow";
        }

        speed += " Attack Speed";

        return speed;
    }

}
