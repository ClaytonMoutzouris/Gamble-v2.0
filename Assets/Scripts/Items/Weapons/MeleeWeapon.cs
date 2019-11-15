using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public MeleeAttackPrototype attack;
    
    public void SetAttack(AttackManager attackManager, int index)
    {
        //attackManager.AttackList[index] = attack;
    }

    public override void OnEquip(Player player)
    {
        base.OnEquip(player);
        player.AttackManager.meleeAttacks[0] = new MeleeAttack(player, this);
        ((PlayerRenderer)player.Renderer).meleeWeapon.sprite = sprite;

    }

    public override void OnUnequip(Player player)
    {
        base.OnUnequip(player);
        player.AttackManager.meleeAttacks[0] = player.defaultMelee;
    }

    public override string getTooltip()
    {
        string tooltip = base.getTooltip();
        tooltip += attack.GetToolTip();

        return tooltip;
    }
}
