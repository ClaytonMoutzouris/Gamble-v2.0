using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AmmoType { Pistol, Launcher, Automatic, Thrower, Charge, Shotgun };

public class RangedWeapon : Weapon
{
    public AmmoType ammoType;
    public RangedAttackPrototype attack;
    [HideInInspector]
    public int ammunitionCount = 5;
    float reloadTimeStamp;
    public bool reloading = false;
    //Measured in shots per second
    float fireTimeStamp;
    public ProjectilePrototype projProto;
    public RangedWeaponAttributes attributes;

    #region baseAttributes
    public List<RangedWeaponAttribute> baseAttributes = new List<RangedWeaponAttribute>{
        new RangedWeaponAttribute(RangedWeaponAttributesType.NumProjectiles, 1),
        new RangedWeaponAttribute(RangedWeaponAttributesType.SpreadAngle, 0),
        new RangedWeaponAttribute(RangedWeaponAttributesType.AmmoCapacity, 5),
        new RangedWeaponAttribute(RangedWeaponAttributesType.ReloadTime, 3),
        new RangedWeaponAttribute(RangedWeaponAttributesType.Damage, 5),
        new RangedWeaponAttribute(RangedWeaponAttributesType.FireRate, 3),
        new RangedWeaponAttribute(RangedWeaponAttributesType.ProjectileSpeed, 100),

    };
    #endregion


    public override void Initialize()
    {
        base.Initialize();
        projProto = Instantiate(projProto);
        SetAttributes();
    }

    public void SetAttributes()
    {
        attributes = new RangedWeaponAttributes();
        attributes.SetStats(baseAttributes);
        ammunitionCount = (int)attributes.GetAttribute(RangedWeaponAttributesType.AmmoCapacity).GetValue();
        //attributes.GetAttribute(RangedWeaponAttributesType.ProjectileSpeed).value = projProto.speed;
        damage = (int)attributes.GetAttribute(RangedWeaponAttributesType.Damage).value;
        projProto.speed = (int)attributes.GetAttribute(RangedWeaponAttributesType.ProjectileSpeed).value;
    }

    public override void OnEquip(Player player)
    {
        SetAttributes();

        base.OnEquip(player);
        if(mSlot == EquipmentSlotType.Mainhand)
        {
            player.AttackManager.rangedAttacks[0] = new RangedAttack(player, attack);
            foreach(Companion companion in player.companionManager.Companions)
            {
                if(companion.copyAttack)
                {
                    companion.mAttackManager.rangedAttacks[0] = new RangedAttack(companion, attack);
                }
            }
            ((PlayerRenderer)player.Renderer).rangedWeapon.sprite = sprite;


        } else
        {
            //player.AttackManager.rangedAttacks[1] = new RangedAttack(player, attack.duration, attack.damage, attack.cooldown, attack.projectile, attack.offset);
            //((PlayerRenderer)player.Renderer).weapon.sprite = sprite;
        }
    }

    public virtual void Fire(Player player)
    {
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

            if(Time.time > fireTimeStamp+(1/ attributes.GetAttribute(RangedWeaponAttributesType.FireRate).GetValue()))
            {


                ammunitionCount--;

                //Debug.Log("Fire " + (ammunitionCapacity - ammunitionCount) + " (Last shot at: " + fireTimeStamp);

                fireTimeStamp = Time.time;

                float interval = attributes.GetAttribute(RangedWeaponAttributesType.SpreadAngle).GetValue() / attributes.GetAttribute(RangedWeaponAttributesType.NumProjectiles).GetValue();

                for (int i = 0; i < attributes.GetAttribute(RangedWeaponAttributesType.NumProjectiles).GetValue(); i++)
                {

                    Vector2 tempDir = player.GetAimRight().normalized;

                    if (attributes.GetAttribute(RangedWeaponAttributesType.NumProjectiles).GetValue() > 1)
                    {
                        tempDir = tempDir.Rotate(-attributes.GetAttribute(RangedWeaponAttributesType.SpreadAngle).GetValue() / 2 + (interval * i));
                    }

                    tempDir.Normalize();
                    RangedAttackPrototype attackProto = Instantiate(attack);
                    attackProto.damage = (int)attributes.GetAttribute(RangedWeaponAttributesType.Damage).GetValue();
                    ProjectilePrototype tempProto = Instantiate(projProto);
                    tempProto.speed = (int)attributes.GetAttribute(RangedWeaponAttributesType.ProjectileSpeed).GetValue();
                    Projectile shot = new Projectile(tempProto, new RangedAttack(player, attackProto), tempDir);
                    shot.Spawn(player.Position + new Vector2(0, attackProto.offset.y) + (attackProto.offset * tempDir.normalized));

                }

                foreach(Companion companion in player.companionManager.Companions)
                {
                    companion.Fire();
                }
                //Actually fire here

            }

            if (ammunitionCount <= 0)
            {
                reloadTimeStamp = Time.time;
                reloading = true;
                Debug.Log("Reloading");

            }
        }


    }

    public void OnUpdate()
    {
        if (reloading && Time.time > reloadTimeStamp + attributes.GetAttribute(RangedWeaponAttributesType.ReloadTime).GetValue())
        {
            reloading = false;
            ammunitionCount = (int)attributes.GetAttribute(RangedWeaponAttributesType.AmmoCapacity).GetValue();
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
        tooltip += "\n<color=white>" + attributes.GetAttribute(RangedWeaponAttributesType.Damage).GetValue() + " Damage</color>";
        tooltip += "\n<color=white>" + AttackSpeedToString() + "</color>";

        return tooltip;
    }

    public string AttackSpeedToString()
    {
        string speed = "";
        float fireRate = attributes.GetAttribute(RangedWeaponAttributesType.FireRate).GetValue();
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
