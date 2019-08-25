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
        player.AttackManager.meleeAttacks[0] = new MeleeAttack(player, attack.duration, attack.damage, attack.cooldown, new Hitbox(player, new CustomAABB(player.Position, attack.hitboxSize, attack.hitboxOffset)), attack.abilities);
    }

    public override void OnUnequip(Player player)
    {
        base.OnUnequip(player);
        player.AttackManager.meleeAttacks[0] = player.defaultMelee;
    }

    public override string getTooltip()
    {
        string tooltip = base.getTooltip();
        tooltip += attack.getToolTip();

        return tooltip;
    }
}
