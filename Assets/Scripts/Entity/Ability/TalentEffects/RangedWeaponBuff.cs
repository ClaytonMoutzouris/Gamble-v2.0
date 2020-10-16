using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RangedWeaponBuff : Ability
{
    public AmmoType weaponClass;
    public int extraProjectiles = 0;
    public int extraAmmoCapacity = 0;
    public float reloadTimeDecrease = 0.0f;
    public int extraDamage = 0;

    public override void OnEquippedTrigger(Entity entity, Equipment equipment)
    {
        base.OnEquippedTrigger(entity, equipment);

        if(equipment is RangedWeapon ranged && ranged.ammoType == weaponClass)
        {
            ranged.numberOfProjectiles += extraProjectiles;
            ranged.ammunitionCapacity += extraAmmoCapacity;
            ranged.ammunitionCount += extraAmmoCapacity;
            ranged.reloadTime -= reloadTimeDecrease;
            ranged.damage += extraDamage;
        } 
    }

    public override void OnUnequipTrigger(Entity entity, Equipment equipment)
    {
        base.OnEquippedTrigger(entity, equipment);

        if (equipment is RangedWeapon ranged && ranged.ammoType == weaponClass)
        {
            ranged.numberOfProjectiles -= extraProjectiles;
            ranged.ammunitionCapacity -= extraAmmoCapacity;
            ranged.ammunitionCount -= extraAmmoCapacity;
            ranged.reloadTime += reloadTimeDecrease;
            ranged.damage -= extraDamage;
        }
    }

    public override void OnRemoveTrigger(Entity entity)
    {
        base.OnRemoveTrigger(entity);

        if (entity is Player player)
        {
            if (player.Equipment.GetSlot(EquipmentSlotType.Mainhand).GetContents() is RangedWeapon ranged && ranged.ammoType == weaponClass)
            {
                ranged.numberOfProjectiles -= extraProjectiles;
                ranged.ammunitionCapacity -= extraAmmoCapacity;
                ranged.ammunitionCount -= extraAmmoCapacity;
                ranged.reloadTime += reloadTimeDecrease;
                ranged.damage -= extraDamage;
            }
        }
    }

    public override void OnGainTrigger(Entity entity)
    {
        base.OnGainTrigger(entity);

        if (entity is Player player)
        {
            //This null checks right?
            if (player.Equipment.GetSlot(EquipmentSlotType.Mainhand).GetContents() is RangedWeapon ranged && ranged.ammoType == weaponClass)
            {
                ranged.numberOfProjectiles += extraProjectiles;
                ranged.ammunitionCapacity += extraAmmoCapacity;
                ranged.ammunitionCount += extraAmmoCapacity;
                ranged.reloadTime -= reloadTimeDecrease;
                ranged.damage += extraDamage;
            }
        }
    }

}
