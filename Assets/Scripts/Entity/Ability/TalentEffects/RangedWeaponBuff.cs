using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RangedWeaponBuff : Ability
{
    public AmmoType weaponClass;
    public List<StatBonus> statBonuses;
    public List<RangedWeaponAttributeBonus> rangedWeaponBonuses;

    public override void OnEquippedTrigger(Entity entity, Equipment equipment)
    {
        base.OnEquippedTrigger(entity, equipment);

        if(equipment is RangedWeapon ranged && ranged.ammoType == weaponClass)
        {

            ranged.attributes.AddBonuses(rangedWeaponBonuses);
            
            if(entity is Player player)
            {
                player.mStats.AddBonuses(statBonuses);
                player.Health.UpdateHealth();

            }
        } 
    }

    public override void OnUnequipTrigger(Entity entity, Equipment equipment)
    {
        base.OnEquippedTrigger(entity, equipment);

        if (equipment is RangedWeapon ranged && ranged.ammoType == weaponClass)
        {
            ranged.attributes.RemoveBonuses(rangedWeaponBonuses);

            if (entity is Player player)
            {
                player.mStats.RemoveBonuses(statBonuses);
                player.Health.UpdateHealth();

            }
        }
    }

    public override void OnRemoveTrigger(Entity entity)
    {
        base.OnRemoveTrigger(entity);

        if (entity is Player player)
        {
            if (player.Equipment.GetSlot(EquipmentSlotType.Mainhand).GetContents() is RangedWeapon ranged && ranged.ammoType == weaponClass)
            {
                ranged.attributes.RemoveBonuses(rangedWeaponBonuses);

                player.mStats.RemoveBonuses(statBonuses);
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
            if (player.Equipment.GetSlot(EquipmentSlotType.Mainhand).GetContents() is RangedWeapon ranged && ranged.ammoType == weaponClass)
            {
                ranged.attributes.AddBonuses(rangedWeaponBonuses);

                player.mStats.AddBonuses(statBonuses);
                player.Health.UpdateHealth();

            }
        }
    }
    public override string GetDescription()
    {
        string tooltip = "";

        foreach (RangedWeaponAttributeBonus bonus in rangedWeaponBonuses)
        {
            tooltip += "\n" + bonus.GetTooltip();
        }
        foreach (StatBonus bonus in statBonuses)
        {
            tooltip += "\n" + bonus.GetTooltip();
        }
        return tooltip;
    }
    //public override Too
}
