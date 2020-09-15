using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : Ability
{
    public int knockbackPower = 500;


    public override void OnHitTrigger(Attack attack, IHurtable entity)
    {
        base.OnHitTrigger(attack, entity);
        if(entity.GetEntity() is Enemy enemy)
        {
            enemy.mEnemyState = EnemyState.Jumping;

        }

        entity.GetEntity().Body.mSpeed = ((int)entity.GetEntity().mDirection*Vector2.left+Vector2.up*0.5f)*500;
    }
}
