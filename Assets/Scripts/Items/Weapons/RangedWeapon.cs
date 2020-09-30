﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public RangedAttackPrototype attack;

    public void SetAttack(AttackManager attackManager, int index)
    {
        //attackManager.AttackList[index] = attack;
    }

    public override void OnEquip(Player player)
    {
        base.OnEquip(player);
        if(mSlot == EquipmentSlotType.Mainhand)
        {
            player.AttackManager.rangedAttacks[0] = new RangedAttack(player, this);
            foreach(Companion companion in player.companionManager.Companions)
            {
                if(companion.copyAttack)
                {
                    companion.mAttackManager.rangedAttacks[0] = new RangedAttack(companion, this);
                }
            }
            ((PlayerRenderer)player.Renderer).rangedWeapon.sprite = sprite;


        } else
        {
            //player.AttackManager.rangedAttacks[1] = new RangedAttack(player, attack.duration, attack.damage, attack.cooldown, attack.projectile, attack.offset);
            //((PlayerRenderer)player.Renderer).weapon.sprite = sprite;
        }
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
