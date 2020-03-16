﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectProjectiles : Effect
{

    public ReflectProjectiles()
    {
        effectName = "Reflect Projectiles";
        //type = EffectType.ReflectProjectiles;
    }

    public override void OnDamagedTrigger(Attack attack)
    {
        base.OnDamagedTrigger(attack);

        if (attack.mEntity is IHurtable attacker)
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