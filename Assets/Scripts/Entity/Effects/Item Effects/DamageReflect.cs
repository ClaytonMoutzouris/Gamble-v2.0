using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReflect : Ability
{
    int procChance = 10;
    public DamageReflect()
    {
        abilityName = "Damage Reflect";
        type = AbilityType.DamageReflect;
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override void OnDamagedTrigger(Attack attack)
    {
        base.OnDamagedTrigger(attack);

        if(Random.Range(0, 100) > procChance)
        {
            return;
        }

        if(attack.mEntity is IHurtable attacker)
        {
            attacker.GetHurt(new Attack(attack.GetDamage()));
        }
    }

    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);
    }

    public override void OnUnequipTrigger(Player player)
    {
        base.OnUnequipTrigger(player);

    }

    public override void OnHitTrigger(Attack attack, IHurtable entity)
    {
        base.OnHitTrigger(attack, entity);
    }

    public override void OnJumpTrigger(Player player)
    {
        base.OnJumpTrigger(player);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
