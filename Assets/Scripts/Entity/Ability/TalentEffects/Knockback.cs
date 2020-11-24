using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : Ability
{
    public int knockbackPower = 500;


    public override void OnHitTrigger(AttackObject attackObject, IHurtable entity)
    {
        base.OnHitTrigger(attackObject, entity);

        if (entity.GetEntity().Body.mIsKinematic || entity.GetEntity().abilityFlags.GetFlag(AbilityFlag.Heavy))
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

        Vector2 knockbackVector;

        if (entity.GetEntity().Body.mPS.pushesBottom)
        {
            knockbackVector = Vector2.right * attackObject.direction.x + Vector2.up * 0.5f;

        }
        else
        {
            knockbackVector = attackObject.direction;
        }

        entity.GetEntity().Body.mSpeed = knockbackVector.normalized*500;
    }
}
