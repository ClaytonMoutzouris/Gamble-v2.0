using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AmmoType { Pistol, Launcher, Automatic, Fuel, Charge, Shotgun };

public class RangedWeapon : Weapon
{
    public AmmoType ammoType;
    public RangedAttackPrototype attack;
    public int ammunitionCapacity = 5;
    public int ammunitionCount = 5;
    public float reloadTime = 3.0f;
    public float reloadCooldown = 0.0f;
    float reloadTimeStamp;
    public bool reloading = false;
    //Measured in shots per second
    public float fireRate = 1.0f;
    float fireTimeStamp;
    public ProjectilePrototype projProto;
    public int numberOfProjectiles = 1;
    public int spreadAngle = 0;

    public override void OnEquip(Player player)
    {
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
        if(reloading && Time.time > reloadTimeStamp+reloadTime)
        {
            reloading = false;
            ammunitionCount = ammunitionCapacity;
            Debug.Log("Reloading Complete");

        }

        if (!reloading && ammunitionCount > 0)
        {

            if(Time.time > fireTimeStamp+(1/fireRate))
            {


                ammunitionCount--;

                //Debug.Log("Fire " + (ammunitionCapacity - ammunitionCount) + " (Last shot at: " + fireTimeStamp);

                fireTimeStamp = Time.time;

                float interval = spreadAngle / numberOfProjectiles;

                for (int i = 0; i < numberOfProjectiles; i++)
                {

                    Vector2 tempDir = player.GetAimRight().normalized;

                    if (numberOfProjectiles > 1)
                    {
                        tempDir = tempDir.Rotate(-spreadAngle / 2 + (interval * i));
                    }

                    tempDir.Normalize();
                    attack.damage = damage;
                    Projectile shot = new Projectile(projProto, new RangedAttack(player, attack), tempDir);
                    shot.Spawn(player.Position + new Vector2(0, attack.offset.y) + (attack.offset * tempDir.normalized));

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
        tooltip += attack.GetToolTip();

        return tooltip;
    }

}
