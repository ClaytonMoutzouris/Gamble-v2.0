using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponBuff : Ability
{
    public WeaponClassType weaponClass;
    public List<StatBonus> statBonuses;
    public List<SecondaryStatBonus> secondaryStatBonuses;
    public List<WeaponAttributeBonus> weaponBonuses;

    public override void OnEquippedTrigger(Entity entity, Equipment equipment)
    {
        base.OnEquippedTrigger(entity, equipment);

        if(equipment is Weapon weapon && (weapon.ammoType == weaponClass || weaponClass == WeaponClassType.All))
        {
            weapon.attributes.AddBonuses(weaponBonuses);

            if (entity is Player player)
            {

                player.mStats.AddPrimaryBonuses(statBonuses);
                player.mStats.AddSecondaryBonuses(secondaryStatBonuses);
                player.Health.UpdateHealth();

            }
        } 
    }

    public override void OnUnequipTrigger(Entity entity, Equipment equipment)
    {
        base.OnEquippedTrigger(entity, equipment);

        if (equipment is Weapon weapon && (weapon.ammoType == weaponClass || weaponClass == WeaponClassType.All))
        {
            weapon.attributes.RemoveBonuses(weaponBonuses);

            if (entity is Player player)
            {
                player.mStats.RemovePrimaryBonuses(statBonuses);
                player.mStats.RemoveSecondaryBonuses(secondaryStatBonuses);
                player.Health.UpdateHealth();

            }
        }
    }

    public override void OnRemoveTrigger(Entity entity)
    {
        base.OnRemoveTrigger(entity);

        if (entity is Player player)
        {
            if (player.Equipment.GetSlot(EquipmentSlotType.Mainhand).GetContents() is RangedWeapon ranged && (ranged.ammoType == weaponClass || weaponClass == WeaponClassType.All))
            {
                ranged.attributes.RemoveBonuses(weaponBonuses);
          
                player.mStats.RemovePrimaryBonuses(statBonuses);
                player.mStats.RemoveSecondaryBonuses(secondaryStatBonuses);

                player.Health.UpdateHealth();

            }

            if (player.Equipment.GetSlot(EquipmentSlotType.Offhand).GetContents() is MeleeWeapon melee && (melee.ammoType == weaponClass || weaponClass == WeaponClassType.All))
            {
                melee.attributes.RemoveBonuses(weaponBonuses);

                player.mStats.RemovePrimaryBonuses(statBonuses);
                player.mStats.RemoveSecondaryBonuses(secondaryStatBonuses);
                player.Health.UpdateHealth();

            }
        }
    }

    public override void OnGainTrigger(Entity entity)
    {
        base.OnGainTrigger(entity);

        if (entity is Player player)
        {
            //This null checks right?
            if (player.Equipment.GetSlot(EquipmentSlotType.Mainhand).GetContents() is RangedWeapon ranged && (ranged.ammoType == weaponClass || weaponClass == WeaponClassType.All))
            {
                ranged.attributes.AddBonuses(weaponBonuses);

                player.mStats.AddPrimaryBonuses(statBonuses);
                player.mStats.AddSecondaryBonuses(secondaryStatBonuses);

                player.Health.UpdateHealth();

            }

            if (player.Equipment.GetSlot(EquipmentSlotType.Offhand).GetContents() is MeleeWeapon melee && (melee.ammoType == weaponClass || weaponClass == WeaponClassType.All))
            {
                melee.attributes.AddBonuses(weaponBonuses);

                player.mStats.AddPrimaryBonuses(statBonuses);
                player.mStats.AddSecondaryBonuses(secondaryStatBonuses);

                player.Health.UpdateHealth();

            }
        }
    }
    public override string GetDescription()
    {
        string tooltip = "";


        foreach (StatBonus bonus in statBonuses)
        {
            tooltip += bonus.GetTooltip() + ", ";
        }
        foreach (SecondaryStatBonus secondaryBonus in secondaryStatBonuses)
        {
            tooltip += secondaryBonus.GetTooltip() + ", ";
        }
        foreach (WeaponAttributeBonus bonus in weaponBonuses)
        {
            tooltip += bonus.GetTooltip() + ", ";
        }
        tooltip = tooltip.Substring(0, tooltip.Length -2);
        tooltip += " for " + weaponClass.ToString() + " weapons.";
        return tooltip;
    }
    //public override Too
}
