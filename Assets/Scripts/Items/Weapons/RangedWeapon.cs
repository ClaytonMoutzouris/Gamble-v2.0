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


}
