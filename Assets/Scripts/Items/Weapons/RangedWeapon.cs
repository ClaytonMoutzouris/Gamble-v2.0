using System.Collections;
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
        player.AttackManager.rangedAttacks[0] = new RangedAttack(player, attack.duration, attack.damage, attack.cooldown, attack.projectile);
    }

    public override void OnUnequip(Player player)
    {
        base.OnUnequip(player);

        //player.AttackManager.rangedAttacks[0] = new RangedAttack(player, player.prototype.duration, rangedAttack.damage, rangedAttack.cooldown, rangedAttack.projectile);
     
    }

}
