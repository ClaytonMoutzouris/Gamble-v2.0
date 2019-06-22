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
        //player.mAttackManager.AttackList[0] = attack;
    }

    public override void OnUnequip(Player player)
    {
        base.OnUnequip(player);

    }
}
