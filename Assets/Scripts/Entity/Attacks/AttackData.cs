using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { Fire, Water, Electric, Physical, Poison, Ice, Void }

public class AttackData : ScriptableObject
{
    public int damage;
    public Vector2 offset;
    public DamageType damageType;
    public bool pureDamage = false;

    public int CalculateDamage(Entity entity)
    {
        int damage = this.damage;

        if (entity is Player player)
        {
            damage += (int)(damage*0.01f*player.mStats.GetSecondaryStat(SecondaryStatType.DamageBonus).GetValue());
        }
        else if (entity is Enemy enemy)
        {
            damage += (int)(damage * 0.01f * enemy.mStats.GetSecondaryStat(SecondaryStatType.DamageBonus).GetValue());
        }

        return damage;
    }

    public AttackData GenerateAttackData(Attack attack, IHurtable hurtable)
    {

        return null;
    }
}
