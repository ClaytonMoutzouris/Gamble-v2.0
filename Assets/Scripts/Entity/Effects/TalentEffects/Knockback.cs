using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : Ability
{
    public Knockback()
    {
        abilityName = "Knockback";
        type = AbilityType.Knockback;
    }

    public override void OnHitTrigger(Attack attack, IHurtable entity)
    {
        base.OnHitTrigger(attack, entity);
        if(entity.GetEntity() is Enemy enemy)
        {
            enemy.mEnemyState = EnemyState.Jumping;

        }

        Vector2 posDifference = entity.GetEntity().Position - attack.mEntity.Position;

        entity.GetEntity().Body.mSpeed = ((int)entity.GetEntity().mDirection*Vector2.left+Vector2.up*0.5f)*500;
    }
}
