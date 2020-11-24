using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MeleeWeaponType { Fist, Blade }

public class MeleeWeapon : Weapon
{
    public MeleeWeaponType weaponType = MeleeWeaponType.Blade;
    public MeleeAttackPrototype attack;
    
    public void SetAttack(AttackManager attackManager, int index)
    {
        //attackManager.AttackList[index] = attack;
    }

    public override void OnEquip(Player player)
    {
        base.OnEquip(player);
        player.AttackManager.meleeAttacks[0] = new MeleeAttack(player, this);

    }

    public override void OnUnequip(Player player)
    {
        base.OnUnequip(player);
        player.AttackManager.meleeAttacks[0] = player.defaultMelee;
    }

    public override string GetTooltip()
    {
        string tooltip = base.GetTooltip();
        tooltip += attack.GetToolTip();

        return tooltip;
    }
}
