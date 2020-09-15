using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReflect : Ability
{
    public int procChance = 10;
    public int reflectPercent = 100;

    public override void OnDamagedTrigger(Attack attack)
    {
        base.OnDamagedTrigger(attack);

        if(Random.Range(0, 100) > procChance)
        {
            return;
        }

        if(attack.mEntity is IHurtable attacker)
        {
            attacker.GetHurt(new Attack(attack.GetDamage()*(reflectPercent/100)));
        }
    }

}
