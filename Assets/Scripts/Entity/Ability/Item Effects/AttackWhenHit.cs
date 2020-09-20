using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWhenHit : Ability
{
    public int procChance = 10;
    public RangedAttackPrototype attackPrototype;
    RangedAttack rangedAttack;

    public override void OnDamagedTrigger(Attack attack)
    {
        base.OnDamagedTrigger(attack);

        if (Random.Range(0, 100) > procChance)
        {
            return;
        }

        rangedAttack = new RangedAttack(owner, attackPrototype);
        Vector2 tempDir = Vector2.up;

        if (attack.mEntity != null)
        {
            tempDir = attack.mEntity.Position - owner.Position;
        } 

        rangedAttack.Activate(tempDir, owner.Position);

    }

}
