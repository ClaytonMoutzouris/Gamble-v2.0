using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : Ability
{
    public int knockbackPower = 500;


    public override void OnHitTrigger(AttackObject attackObject, IHurtable entity)
    {
        base.OnHitTrigger(attackObject, entity);

        if (entity.GetEntity().abilityFlags.GetFlag(AbilityFlag.Heavy))
        {
            return;
        }

        if(entity.GetEntity() is Enemy enemy)
        {


            enemy.mEnemyState = EnemyState.Jumping;

        }

        if (entity.GetEntity() is Player player)
        {


            player.movementState = MovementState.Knockback;

        }

        entity.GetEntity().Body.mSpeed = (attackObject.direction).normalized*500;
    }
}
